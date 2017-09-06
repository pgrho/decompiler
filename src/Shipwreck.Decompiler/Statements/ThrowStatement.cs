using System.CodeDom.Compiler;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class ThrowStatement : Statement, IBreakingStatement
    {
        public ThrowStatement()
        {
        }

        public ThrowStatement(Expression value)
        {
            Value = value;
        }

        public Expression Value { get; set; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is ThrowStatement rs && (Value?.IsEquivalentTo(rs?.Value) ?? rs.Value == null));

        public override void WriteTo(IndentedTextWriter writer)
        {
            writer.Write("throw");
            if (Value != null)
            {
                writer.Write(' ');
                Value.WriteTo(writer);
            }
            writer.WriteLine(';');
        }

        public override bool Reduce()
        {
            if (this.TruReduceReturnValue(Value, out var ov))
            {
                Value = ov;
                return true;
            }

            return true;
        }

        public override Statement Clone()
            => new ThrowStatement(Value);
    }
}