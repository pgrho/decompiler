using Shipwreck.CSharpModels.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadStringInstruction : LoadConstantInstruction
    {
        public LoadStringInstruction(string value)
        {
            Value = value;
        }

        public string Value { get; }

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = new ConstantExpression(Value);
            return true;
        }

        public override bool IsEqualTo(Instruction other)
            => this == (object)other
            && (other is LoadStringInstruction li && Value == li.Value);

        public override string ToString()
            => $"ldstr {Value:r}";
    }
}