using Shipwreck.CSharpModels.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadDoubleInstruction : LoadConstantInstruction
    {
        public LoadDoubleInstruction(double value)
        {
            Value = value;
        }

        public double Value { get; }

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = new ConstantExpression(Value);
            return true;
        }

        public override bool IsEqualTo(Instruction other)
            => this == (object)other
            && (other is LoadDoubleInstruction li && Value == li.Value);

        public override string ToString()
            => $"ldc.r8 {Value:r}";
    }
}