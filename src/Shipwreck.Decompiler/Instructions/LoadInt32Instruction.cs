using System.Reflection.Emit;
using Shipwreck.CSharpModels.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadInt32Instruction : LoadConstantInstruction
    {
        public LoadInt32Instruction(int value)
        {
            Value = value;
        }

        public int Value { get; }

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = new ConstantExpression(Value);
            return true;
        }

        public override bool IsEqualTo(Instruction other)
            => this == (object)other
            && (other is LoadInt32Instruction li && Value == li.Value);

        public override string ToString()
            => $"ldc.i4 {Value:d}";
    }
}