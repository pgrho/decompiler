using System;
using System.Reflection.Emit;
using System.Text;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class BranchBinaryInstruction : Instruction
    {
        public BranchBinaryInstruction(int target, BinaryOperator @operator, bool unsigned = false)
        {
            Target = target;
            Operator = @operator;
            IsUnsigned = unsigned;
        }

        public override int PopCount
            => 2;

        internal bool TryCreateOperands(DecompilationContext context, ref int index, out Expression left, out Expression right)
        {
            if (index > 1 && context.GetFromCount(this) <= 1)
            {
                var j = index - 1;
                if (context.TryCreateExpression(ref j, out right))
                {
                    j--;
                    if (j >= 0)
                    {
                        if (context.TryCreateExpression(ref j, out left))
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

        public int Target { get; }

        public BinaryOperator Operator { get; }

        public bool IsUnsigned { get; }

        public override FlowControl FlowControl
            => FlowControl.Cond_Branch;

        public override int PushCount
            => 0;

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = null;
            return false;
        }

        internal override bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            if (TryCreateOperands(context, ref startIndex, out var l, out var r))
            {
                var ib = new IfStatement(IsUnsigned ? l.AsUnsigned().MakeBinary(r.AsUnsigned(), Operator) : l.MakeBinary(r, Operator));
                ib.TruePart.Add(new TemporalGoToStatement(Target));
                statement = ib;
                return true;
            }
            statement = null;
            return false;
        }

        public override bool IsEquivalentTo(Syntax other)
            => other is BranchBinaryInstruction bi
                && Target == bi.Target
                && Operator == bi.Operator
                && IsUnsigned == bi.IsUnsigned;

        public override string ToString()
        {
            var sb = new StringBuilder();

            switch (Operator)
            {
                case BinaryOperator.Equal:
                    sb.Append("beq");
                    break;

                case BinaryOperator.NotEqual:
                    sb.Append("bne");
                    break;

                case BinaryOperator.GreaterThan:
                    sb.Append("bgt");
                    break;

                case BinaryOperator.GreaterThanOrEqual:
                    sb.Append("bge");
                    break;

                case BinaryOperator.LessThan:
                    sb.Append("blt");
                    break;

                case BinaryOperator.LessThanOrEqual:
                    sb.Append("ble");
                    break;

                default:
                    throw new NotImplementedException();
            }

            if (IsUnsigned)
            {
                sb.Append(".un");
            }

            sb.Append(" L_");
            sb.Append(Target.ToString("x4"));

            return sb.ToString();
        }
    }
}