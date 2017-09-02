using System.CodeDom.Compiler;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class IfBlock : Statement
    {
        public IfBlock(Expression condition)
        {
            condition.ArgumentIsNotNull(nameof(condition));

            Condition = condition;
        }

        public Expression Condition { get; }

        #region TruePart

        private StatementCollection _TruePart;

        public StatementCollection TruePart
            => _TruePart ?? (_TruePart = new StatementCollection());

        #endregion TruePart

        #region FalsePart

        private StatementCollection _FalsePart;

        public StatementCollection FalsePart
            => _FalsePart ?? (_FalsePart = new StatementCollection());

        #endregion FalsePart

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is IfBlock gt
                && Condition.IsEquivalentTo(gt.Condition)
                && _TruePart.IsEquivalentTo(gt._TruePart)
                && _FalsePart.IsEquivalentTo(gt._FalsePart));

        public override void WriteTo(IndentedTextWriter writer)
        {
            writer.Write("if (");
            Condition.WriteTo(writer);
            writer.WriteLine(')');

            writer.WriteLine('{');
            if (_TruePart?.Count > 0)
            {
                writer.Indent++;
                foreach (var s in _TruePart)
                {
                    s.WriteTo(writer);
                }
                writer.Indent--;
            }
            writer.WriteLine('}');

            if (_FalsePart?.Count > 0)
            {
                if (_FalsePart.Count == 1 && _FalsePart[0] is IfBlock cib)
                {
                    writer.Write("else ");
                    cib.WriteTo(writer);
                }
                else
                {
                    writer.WriteLine("else");
                    writer.WriteLine('{');
                    writer.Indent++;
                    foreach (var s in _FalsePart)
                    {
                        s.WriteTo(writer);
                    }
                    writer.Indent--;
                    writer.WriteLine('}');
                }
            }
        }
    }
}