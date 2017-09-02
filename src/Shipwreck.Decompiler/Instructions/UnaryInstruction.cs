using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class UnaryInstruction : Instruction
    {
        public UnaryInstruction(UnaryOperator @operator)
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

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            if (context.GetFromCount(this) <= 1 && index > 0)
            {
                var j = index - 1;
                var prev = context.RootStatements[j] as Instruction;
                if (prev != null && prev.TryCreateExpression(context, ref j, out var e))
                {
                    index = j;
                    expression = e.MakeUnary(Operator);
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

        public override bool IsEquivalentTo(Syntax other)
            => other is UnaryInstruction ui
                && Operator == ui.Operator;

        public override string ToString()
        {
            var sb = new StringBuilder();

            switch (Operator)
            {
                case UnaryOperator.Negate:
                    sb.Append("neg");
                    break;

                case UnaryOperator.Not:
                    sb.Append("not");
                    break;

                default:
                    throw new NotImplementedException();
            }

            return sb.ToString();
        }
    }
}