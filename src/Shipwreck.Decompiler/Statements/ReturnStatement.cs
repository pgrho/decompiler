using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class ReturnStatement : Statement
    {
        public ReturnStatement(Expression value)
        {
            Value = value;
        }

        public Expression Value { get; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is ReturnStatement rs && ((Value == null && rs.Value == null) || (Value?.IsEquivalentTo(rs?.Value) == true)));
    }

}