using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class NotInstruction : UnaryInstruction
    {
        internal override Expression MakeUnary(Expression expression)
            => expression.Not();

        public override bool IsEquivalentTo(Syntax other)
            => other is NotInstruction;
    }
}