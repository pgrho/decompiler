using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class UnaryInstruction : Instruction
    {
        public UnaryInstruction(int offset, UnaryOperator @operator)
            : base(offset)
        {
            Operator = @operator;
        }

        public UnaryOperator Operator { get; }

        public override FlowControl FlowControl
            => FlowControl.Next;

        public override int PopCount
            => 1;

        public override int PushCount
            => 1;

        internal override bool TryCreateExpression(MethodBase method, List<Syntax> list, ref int index, out Expression expression)
        {
            if (index > 0)
            {
                var j = index - 1;
                var prev = list[j] as Instruction;
                if (prev != null && prev.TryCreateExpression(method, list, ref j, out var e))
                {
                    index = j;
                    expression = e.MakeUnary(Operator);
                    return true;
                }
            }
            expression = null;
            return false;
        }

        internal override bool TryCreateStatement(MethodBase method, List<Syntax> list, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            statement = null;
            return false;
        }

        public override bool IsEquivalentTo(Syntax other)
            => other is UnaryInstruction ui
                && Operator == ui.Operator;
    }
}