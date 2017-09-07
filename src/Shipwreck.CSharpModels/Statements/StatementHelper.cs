using System.Collections.Generic;
using System.Linq;
using Shipwreck.CSharpModels.Expressions;

namespace Shipwreck.CSharpModels.Statements
{
    public static class StatementHelper
    {
        #region Traversing

        public static IEnumerable<Statement> TreeStatements(this IStatementNode statement)
            => statement.Collection?.Owner?.TreeStatements()
                ?? statement.Collection?.Descendants()
                ?? statement.SelfAndDescendants();

        public static IEnumerable<Statement> SelfAndDescendants(this IStatementNode statements)
        {
            if (statements is Statement s)
            {
                yield return s;
            }

            foreach (var c in statements.GetChildCollections())
            {
                foreach (var cs in c.Descendants())
                {
                    yield return cs;
                }
            }
        }

        public static IEnumerable<Statement> Descendants(this StatementCollection collection)
            => collection.SelectMany(s => s.SelfAndDescendants());

        public static IEnumerable<Statement> Ancestors(this Statement statement)
        {
            var c = statement.Collection;
            while (c != null)
            {
                var s = c.Owner as Statement;
                if (s == null)
                {
                    yield break;
                }
                yield return s;

                c = s.Collection;
            }
        }

        #endregion Traversing

        internal static bool HasLabel(this Statement s)
            => s.SelfAndDescendants().OfType<LabelTarget>().Any();

        public static void ReplaceBy(this Statement s, IEnumerable<Statement> newStatements)
        {
            var rc = s.Collection;
            var ri = rc.IndexOf(s);
            rc.RemoveAt(ri);
            rc.InsertRange(ri, newStatements);
        }

        internal static bool IsBreaking(this Statement s, bool allowBreak = true)
        {
            if (s == null)
            {
                return false;
            }
            if (s is IBreakingStatement)
            {
                return true;
            }

            if (!(s is IIterationStatement))
            {
                if (s is TryStatement ts)
                {
                    var l = ts.Block.LastOrDefault();
                    if (!l.IsBreaking(allowBreak) || (!allowBreak && l is BreakStatement))
                    {
                        return false;
                    }
                    return true;
                }
                else
                {
                    allowBreak &= !(s is IBreakableStatement);
                    var hasCollection = false;
                    foreach (var sc in s.GetChildCollections())
                    {
                        var l = sc.LastOrDefault();
                        if (!l.IsBreaking(allowBreak) || (!allowBreak && l is BreakStatement))
                        {
                            return false;
                        }
                        hasCollection = true;
                    }
                    return hasCollection;
                }
            }

            return false;
        }

        internal static bool TruReduceReturnValue(this Statement statement, Expression value, out Expression reducedValue)
        {
            // TODO: traverse finally blocks
            var reduced = false;
            reducedValue = value;

            if (reducedValue != null)
            {
                if (reducedValue.TryReduce(out var e))
                {
                    reducedValue = e;
                    reduced = true;
                }

                if (reducedValue is AssignmentExpression vae
                    && vae.Left.IsLocalVariable())
                {
                    reducedValue = vae.Right;
                    reduced = true;
                }
            }

            var i = statement?.Collection.IndexOf(statement) ?? -1;
            if (i > 0
                && reducedValue != null
                && statement.Collection[i - 1] is ExpressionStatement pes
                && pes.Expression is AssignmentExpression pae
                && pae.Left.IsLocalVariable())
            {
                // TODO: consider side effect
                if (!reducedValue.EnumeratePostOrder().Any(n => n is AssignmentExpression ne && ne.Left.IsEqualTo(pae.Left))
                    && reducedValue.EnumeratePostOrder().Count(n => n.IsEqualTo(pae.Left)) == 1)
                {
                    if (reducedValue.TryReplace(pae.Left, pae.Right, out var replaced))
                    {
                        statement.Collection.RemoveAt(i - 1);
                        reducedValue = replaced;

                        return true;
                    }
                }
            }

            reducedValue = reduced ? reducedValue : null;
            return reduced;
        }
    }
}