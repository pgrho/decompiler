using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{

    public sealed class LoadArgumentInstruction : LoadIndexInstruction
    {
        public LoadArgumentInstruction(int index)
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

            return true;
        }

        public override bool IsEquivalentTo(Syntax other)
            => other is LoadArgumentInstruction li && Index == li.Index;

        public override string ToString()
            => $"ldarg {Index}";
    }
}