using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadInt64Instruction : LoadConstantInstruction
    {
        public LoadInt64Instruction(long value)
        {
            Value = value;
        }

        public long Value { get; }

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = new ConstantExpression(Value);
            return true;
        }

        public override bool IsEqualTo(Instruction other)
            => this == (object)other
            && (other is LoadInt64Instruction li && Value == li.Value);

        public override string ToString()
            => $"ldc.i8 {Value:d}";
    }
}