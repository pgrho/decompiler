using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed partial class ThrowStatement : Statement, IBreakingStatement
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