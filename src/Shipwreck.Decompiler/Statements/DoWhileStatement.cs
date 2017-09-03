using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class DoWhileStatement : Statement, IBreakableStatement, IContinuableStatement
    {
        public DoWhileStatement()
        {
            Condition = ExpressionBuilder.True;
        }

        public DoWhileStatement(Expression condition)
        {
            Condition = condition ?? ExpressionBuilder.True;
        }

        public Expression Condition { get; set; }

        #region Statements

        private StatementCollection _Statements;

        public StatementCollection Statements
            => _Statements ?? (_Statements = new StatementCollection(this));

        public bool ShouldSerializeStatements()
            => _Statements.ShouldSerialize();

        #endregion Statements

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is DoWhileStatement dw
                && Condition.IsEquivalentTo(dw.Condition)
                && _Statements.IsEquivalentTo(dw._Statements));

        public override void WriteTo(IndentedTextWriter writer)
        {
            writer.WriteLine("do");
            writer.WriteLine('{');
            if (ShouldSerializeStatements())
            {
                writer.Indent++;
                foreach (var s in _Statements)
                {
                    s.WriteTo(writer);
                }
                writer.Indent--;
            }
            writer.Write("} while (");
            Condition.WriteTo(writer);
            writer.WriteLine(");");
        }

        public override IEnumerable<StatementCollection> GetChildCollections()
        {
            if (ShouldSerializeStatements())
            {
                yield return _Statements;
            }
        }

        public override bool Reduce()
        {
            var thisReduced = Condition.TryReduce(out var nc);
            if (thisReduced)
            {
                Condition = nc;
            }

            if (Collection != null)
            {
                if (Condition is ConstantExpression c)
                {
                    // TODO: DoWhileStatement.Condition is constant
                }
                else
                {
                    if (_Statements?.LastOrDefault() is ExpressionStatement es
                        && es.Expression is AssignmentExpression ae
                        && Condition.TryReplace(ae.Left, ae, out var replaced))
                    {
                        _Statements.RemoveAt(_Statements.Count - 1);
                        Condition = replaced;

                        return true;
                    }
                }

                var i = Collection.IndexOf(this);

                if (i > 0
                    && Collection[i - 1] is GoToStatement gs
                    && _Statements?.LastOrDefault() is LabelTarget lb
                    && gs.Target == lb)
                {
                    var sts = _Statements.ToArray();
                    var w = new WhileStatement(Condition);
                    _Statements.Clear();
                    w.Statements.AddRange(sts);

                    var ct = Collection;
                    ct.RemoveAt(i);
                    ct[i - 1] = w;

                    return true;
                }

                bool iterReduced;
                do
                {
                    iterReduced = false;
                    if (_Statements != null)
                    {
                        foreach (var s in _Statements)
                        {
                            if (s.Reduce())
                            {
                                thisReduced = iterReduced = true;
                                break;
                            }
                        }
                    }
                } while (iterReduced);

                // TODO: Determine the Statements is empty
            }

            return thisReduced;
        }
    }
}