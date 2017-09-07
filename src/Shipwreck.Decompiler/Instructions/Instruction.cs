using System;
using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public abstract class Instruction
    {
        public abstract FlowControl FlowControl { get; }

        public abstract int PopCount { get; }

        public abstract int PushCount { get; }

        internal abstract bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression);

        internal abstract bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement);

        internal virtual void SetTo(DecompilationContext context, int index)
        {
            switch (FlowControl)
            {
                case FlowControl.Next:
                    context.SetTo(this, context.RootStatements[index + 1]);
                    return;

                case FlowControl.Return:
                    context.ClearTo(this);
                    return;
            }
            throw new NotImplementedException();
        }
        public abstract bool IsEqualTo(Instruction other);
    }
}