using System;
using System.Reflection.Emit;
using System.Text;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class BinaryInstruction : Instruction
    {
        public BinaryInstruction(BinaryOperator @operator, bool unsigned = false)
        {
            Operator = @operator;
            IsUnsigned = unsigned;
        }

        public BinaryOperator Operator { get; }

        public bool IsUnsigned { get; }

        public override FlowControl FlowControl
            => FlowControl.Next;

        public override int PopCount
            => 2;

        public override int PushCount
            => 1;

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            if (index > 1 && context.GetFromCount(this) <= 1)
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
                            expression = left.MakeBinary(right, Operator);
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
            => other is BinaryInstruction bi
                && Operator == bi.Operator
                && IsUnsigned == bi.IsUnsigned;

        public override string ToString()
        {
            var sb = new StringBuilder();

            switch (Operator)
            {
                case BinaryOperator.Add:
                    sb.Append("add");
                    break;

                case BinaryOperator.AddChecked:
                    sb.Append("add.ovf");
                    break;

                case BinaryOperator.Subtract:
                    sb.Append("sub");
                    break;

                case BinaryOperator.SubtractChecked:
                    sb.Append("sub.ovf");
                    break;

                case BinaryOperator.Multiply:
                    sb.Append("mul");
                    break;

                case BinaryOperator.MultiplyChecked:
                    sb.Append("mul.ovf");
                    break;

                case BinaryOperator.Divide:
                    sb.Append("div");
                    break;

                default:
                    throw new NotImplementedException();
            }

            if (IsUnsigned)
            {
                sb.Append(".un");
            }

            return sb.ToString();
        }
    }
}