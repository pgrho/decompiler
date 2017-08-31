using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class NegateInstruction : UnaryInstruction
    {
        internal override Expression MakeUnary(Expression expression)
            => expression.Negate();

        public override bool IsEquivalentTo(Syntax other)
            => other is NegateInstruction;
    }
}