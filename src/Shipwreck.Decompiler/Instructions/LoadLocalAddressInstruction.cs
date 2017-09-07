using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadLocalAddressInstruction : LoadIndexInstruction
    {
        public LoadLocalAddressInstruction(int index)
            : base(index)
        {
        }

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = new VariableExpression(Index, context.Method.GetMethodBody().LocalVariables[Index].LocalType).AddressOf();
            return true;
        }

        public override bool IsEqualTo(Instruction other)
            => this == (object)other
            || (other is LoadLocalAddressInstruction li && Index == li.Index);

        public override string ToString()
            => $"ldloca {Index}";
    }
}