using System.CodeDom.Compiler;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class ReturnStatement : Statement
    {
        public ReturnStatement(Expression value)
        {
            Value = value;
        }

        public Expression Value { get; set; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is ReturnStatement rs && ((Value == null && rs.Value == null) || (Value?.IsEquivalentTo(rs?.Value) == true)));

        public override void WriteTo(IndentedTextWriter writer)
        {
            writer.Write("return");
            if (Value != null)
            {
                writer.Write(' ');
                Value.WriteTo(writer);
            }
            writer.WriteLine(';');
        }

        public override bool Reduce()
        {
            if (Value != null && Value.TryReduce(out var e))
            {
                Value = e;
                return true;
            }
            return base.Reduce();
        }
    }
}