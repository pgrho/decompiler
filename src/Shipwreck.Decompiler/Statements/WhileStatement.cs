using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed partial class WhileStatement : Statement, IBreakableStatement, IContinuableStatement
    {
        public WhileStatement()
        {
            Condition = ExpressionBuilder.True;
        }

        public WhileStatement(Expression condition)
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
            || (other is WhileStatement ws
                && Condition.IsEquivalentTo(ws.Condition)
                && _Statements.IsEquivalentTo(ws._Statements));

        public override void WriteTo(IndentedTextWriter writer)
        {
            writer.Write("while (");
            Condition.WriteTo(writer);
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
            var thisReduced = Condition.TryReduce(out var nc);
            if (thisReduced)
            {
                Condition = nc;
            }

            if (Collection != null)
            {
                if (Condition is ConstantExpression c)
                {
                    // TODO: WhileStatement.Condition is constant
                }

                var ls = _Statements?.LastOrDefault() as ExpressionStatement;
                if (ls != null)
                {
                    // Include initializer or second to last label
                    var i = Collection.IndexOf(this);

                    var sts = _Statements.Take(_Statements.Count - 1).ToArray();
                    var fs = new ForStatement();

                    fs.Condition = Condition;
                    fs.Iterator = ls.Expression;

                    _Statements.Clear();
                    fs.Statements.AddRange(sts);

                    var ct = Collection;
                    ct[i] = fs;

                    return true;
                }

                bool iterReduced;
                do
                {
                    iterReduced = _Statements.ReduceBlock();
                    thisReduced |= iterReduced;
                }
                while (iterReduced);

                // TODO: Determine the Statements is empty
            }

            return thisReduced;
        }
        public override Statement Clone()
        {
            var r = new WhileStatement(Condition);
            if (ShouldSerializeStatements())
            {
                r.Statements.AddRange(_Statements.Select(s => s.Clone()));
            }
            return r;
        }
    }
}