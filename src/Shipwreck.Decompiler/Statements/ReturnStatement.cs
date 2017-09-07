using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed partial class ReturnStatement : Statement, IBreakingStatement
    {
        public ReturnStatement()
        {
        }

        public ReturnStatement(Expression value)
        {
            Value = value;
        }

        public Expression Value { get; set; }

        public override bool IsEqualTo(Syntax other)
            => this == (object)other
            || (other is ReturnStatement rs && ((Value == null && rs.Value == null) || (Value?.IsEqualTo(rs?.Value) == true)));

        public override bool Reduce()
        {
            if (this.TruReduceReturnValue(Value, out var ov))
            {
                Value = ov;
                return true;
            }

            return false;
        }

        public override Statement Clone()
            => new ReturnStatement(Value);
    }
}