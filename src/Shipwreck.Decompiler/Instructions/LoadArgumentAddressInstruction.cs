using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadArgumentAddressInstruction : LoadIndexInstruction
    {
        public LoadArgumentAddressInstruction(int index)
            : base(index)
        {
        }

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            if (context.Method.IsStatic)
            {
                expression = context.GetParameter(Index);
            }
            else if (Index == 0)
            {
                expression = context.This;
            }
            else
            {
                expression = context.GetParameter(Index - 1);
            }
            expression = expression.AddressOf();

            return true;
        }

        public override bool IsEqualTo(Syntax other)
            => other is LoadArgumentAddressInstruction li && Index == li.Index;

        public override string ToString()
            => $"ldarga {Index}";
    }
}