using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class ForStatement : Statement, IBreakableStatement, IContinuableStatement
    {
        // TODO: allow variable declaration
        public Expression Initializer { get; set; }

        public Expression Condition { get; set; }
        public Expression Iterator { get; set; }

        #region Statements

        private StatementCollection _Statements;

        public StatementCollection Statements
            => _Statements ?? (_Statements = new StatementCollection(this));

        public bool ShouldSerializeStatements()
            => _Statements.ShouldSerialize();

        #endregion Statements

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is ForStatement ws
                && (Initializer?.IsEquivalentTo(ws.Initializer) ?? ws.Initializer == null)
                && (Condition?.IsEquivalentTo(ws.Condition) ?? ws.Condition == null)
                && (Iterator?.IsEquivalentTo(ws.Iterator) ?? ws.Iterator == null)
                && _Statements.IsEquivalentTo(ws._Statements));

        public override void WriteTo(IndentedTextWriter writer)
        {
            writer.Write("for (");
            Initializer?.WriteTo(writer);
            writer.Write("; ");
            Condition?.WriteTo(writer);
            writer.Write("; ");
            Iterator?.WriteTo(writer);
            writer.WriteLine(")");
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
            writer.WriteLine('}');
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
            var thisReduced = false;
            if (Iterator != null)
            {
                if (Iterator.TryReduce(out var e))
                {
                    Iterator = e;
                    thisReduced = true;
                }
            }
            if (Condition != null)
            {
                if (Condition.TryReduce(out var e))
                {
                    Condition = e;
                    thisReduced = true;
                }
                else if (Condition is ConstantExpression ce && true.Equals(ce.Value))
                {
                    Condition = null;
                    thisReduced = true;
                }
            }
            if (Iterator != null)
            {
                if (Iterator.TryReduce(out var e))
                {
                    Iterator = e;
                    thisReduced = true;
                }
            }

            if (Collection != null)
            {
                if (Condition is ConstantExpression c)
                {
                    // TODO: WhileStatement.Condition is constant
                }

                bool iterReduced;
                do
                {
                    iterReduced = _Statements.ReduceBlock();
                    thisReduced |= iterReduced;
                }
                while (iterReduced);

                if (Collection.GetPreviousOf(this, out var i) is GoToStatement gt)
                {
                    var j = Statements.IndexOf(gt.Target);
                    if (j == 0)
                    {
                        Collection.Remove(gt);
                        return true;
                    }
                    else if (j > 0 && !Statements.Skip(j + 1).OfType<LabelTarget>().Any())
                    {
                        var sts = Statements.Skip(j + 1).Select(s => s.Clone()).ToArray();
                        Collection.RemoveAt(i - 1);
                        Collection.InsertRange(i - 1, sts);
                        return true;
                    }
                }

                // TODO: Determine the Statements is empty
            }

            return thisReduced;
        }

        public override Statement Clone()
        {
            var r = new ForStatement()
            {
                Initializer = Initializer,
                Condition = Condition,
                Iterator = Iterator
            };

            if (ShouldSerializeStatements())
            {
                r.Statements.AddRange(_Statements.Select(s => s.Clone()));
            }
            return r;
        }
    }
}