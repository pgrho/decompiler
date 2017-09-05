using System;
using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public abstract class UnaryExpressionInstruction : Instruction
    {
        public override FlowControl FlowControl
            => FlowControl.Next;

        public override int PopCount
            => 1;

        public override int PushCount
            => 1;

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            if (context.GetFromCount(this) <= 1 && index >= 1)
            {
                var j = index - 1;

                if (context.TryCreateExpression(ref j, out var e))
                {
                    index = j;
                    expression = CreateExpression(context, e);
                    return true;
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

        internal abstract Expression CreateExpression(DecompilationContext context, Expression value);
    }

}