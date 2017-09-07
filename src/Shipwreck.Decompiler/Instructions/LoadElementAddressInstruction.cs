using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadElementAddressInstruction : BinaryExpressionInstruction
    {
        internal override Expression CreateExpression(Expression arg1, Expression arg2)
            => arg1.MakeIndex(arg2).AddressOf();

        public override bool IsEqualTo(Instruction other)
            => this == other || other is LoadElementAddressInstruction;

        public override string ToString()
            => "ldelema";
    }
}