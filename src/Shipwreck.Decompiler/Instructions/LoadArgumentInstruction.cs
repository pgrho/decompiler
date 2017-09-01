using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadArgumentInstruction : Instruction
    {
        public LoadArgumentInstruction(int value)
        {
            Index = value;
        }

        public int Index { get; }

        public override FlowControl FlowControl
            => FlowControl.Next;

        public override int PopCount
            => 0;

        public override int PushCount
            => 1;

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            if (context.Method.IsStatic)
            {
                expression = new ParameterExpression(Index);
            }
            else if (Index == 0)
            {
                expression = new ThisExpression();
            }
            else
            {
                expression = new ParameterExpression(Index - 1);
            }

            return true;
        }

        internal override bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            statement = null;
            return false;
        }

        public override bool IsEquivalentTo(Syntax other)
            => other is LoadArgumentInstruction li && Index == li.Index;

        public override string ToString()
            => $"ldarg {Index}";
    }
}