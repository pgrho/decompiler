using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public abstract class BinaryInstructionBase : Instruction
    {
        public override int PopCount
            => 2;

        internal bool TryCreateOperands(DecompilationContext context, ref int index, out Expression left, out Expression right)
        {
            if (index > 1 && context.GetFromCount(this) <= 1)
            {
                var j = index - 1;
                if (context.TryCreateExpression(ref j, out right))
                {
                    j--;
                    if (j >= 0)
                    {
                        if (context.TryCreateExpression(ref j, out left))
                        {
                            index = j;

                            return true;
                        }
                    }
                }
            }
            left = right = null;
            return false;
        }
    }
}