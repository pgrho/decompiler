using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadElementInstruction : Instruction
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
                var prev = context.RootStatements[j] as Instruction;
                if (prev != null && prev.TryCreateExpression(context, ref j, out var right))
                {
                    j--;
                    if (j >= 0)
                    {
                        prev = context.RootStatements[j] as Instruction;
                        if (prev != null && prev.TryCreateExpression(context, ref j, out var left))
                        {
                            index = j;
                            expression = left.ArrayIndex(right);
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

        public override bool IsEquivalentTo(Syntax other)
            => other is LoadElementInstruction;

        public override string ToString()
            => "ldelem";
    }
}