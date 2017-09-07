using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadNullInstruction : LoadConstantInstruction
    {
        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = new ConstantExpression(null);
            return true;
        }

        public override bool IsEqualTo(Instruction other)
            => other is LoadNullInstruction;

        public override string ToString()
            => "ldnull";
    }
}