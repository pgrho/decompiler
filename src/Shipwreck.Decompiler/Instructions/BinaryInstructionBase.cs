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
                var prev = context.RootStatements[j] as Instruction;
                if (prev != null && prev.TryCreateExpression(context, ref j, out right))
                {
                    j--;
                    if (j >= 0)
                    {
                        prev = context.RootStatements[j] as Instruction;
                        if (prev != null && prev.TryCreateExpression(context, ref j, out left))
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