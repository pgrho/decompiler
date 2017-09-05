using System.Collections.Generic;
using System.Linq;

namespace Shipwreck.Decompiler.Statements
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
    }
}