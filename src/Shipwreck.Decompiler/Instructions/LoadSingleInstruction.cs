using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadSingleInstruction : LoadConstantInstruction
    {
        public LoadSingleInstruction(float value)
        {
            Value = value;
        }

        public float Value { get; }

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = new ConstantExpression(Value);
            return true;
        }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            && (other is LoadSingleInstruction li && Value == li.Value);

        public override string ToString()
            => $"ldc.r4 {Value:r}";
    }
}