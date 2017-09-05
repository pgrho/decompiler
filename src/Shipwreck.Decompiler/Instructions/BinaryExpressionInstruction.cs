using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public abstract class BinaryExpressionInstruction : Instruction
    {
        public override FlowControl FlowControl
            => FlowControl.Next;

        public override int PopCount
            => 2;

        public override int PushCount
            => 1;

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            if (context.GetFromCount(this) <= 1 && index > 1)
            {
                var j = index - 1;
                if (context.TryCreateExpression(ref j, out var right))
                {
                    j--;
                    if (j >= 0)
                    {
                        if (context.TryCreateExpression(ref j, out var left))
                        {
                            index = j;
                            expression = CreateExpression(left, right);
                            return true;
                        }
                    }
                }
            }
            expression = null;
            return false;
        }

        internal override bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            statement = null;
            return false;
        }

        internal abstract Expression CreateExpression(Expression arg1, Expression arg2);
    }
}