using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public abstract class StoreIndexInstruction : Instruction
    {
        public StoreIndexInstruction(int index)
        {
            Index = index;
        }

        public int Index { get; }

        public override FlowControl FlowControl
            => FlowControl.Next;

        public override int PopCount
            => 1;

        public override int PushCount
            => 0;

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            // If previous instruction is dup

            if (context.Flow[index].FromCount <= 1 && index >= 2)
            {
                if (context.Flow[index - 1].Syntax is DuplicateInstruction)
                {
                    var j = index - 2;

                    var prev = context.Flow[j].Syntax as Instruction;
                    if (prev != null && prev.TryCreateExpression(context, ref j, out var e))
                    {
                        index = j;
                        expression = CreateStoreExpression(context, e);
                        return true;
                    }
                }
            }

            expression = null;
            return false;
        }

        internal override bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            // If previous instruction is not dup
            if (context.Flow[startIndex].FromCount <= 1 && startIndex >= 1)
            {
                var j = startIndex - 1;

                var prev = context.Flow[j].Syntax as Instruction;
                if (prev != null && prev.TryCreateExpression(context, ref j, out var e))
                {
                    startIndex = j;
                    statement = CreateStoreExpression(context, e).ToStatement();
                    return true;
                }
            }

            statement = null;
            return false;
        }

        internal abstract Expression CreateStoreExpression(DecompilationContext context, Expression value);
    }
}