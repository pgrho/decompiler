using System;
using System.Text;
using Shipwreck.CSharpModels.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class BinaryInstruction : BinaryExpressionInstruction
    {
        public BinaryInstruction(BinaryOperator @operator, bool unsigned = false)
        {
            Operator = @operator;
            IsUnsigned = unsigned;
        }

        public BinaryOperator Operator { get; }

        public bool IsUnsigned { get; }

        internal override Expression CreateExpression(Expression arg1, Expression arg2)
            => (IsUnsigned ? arg1.AsUnsigned() : arg1)
                .MakeBinary(
                    (IsUnsigned && !Operator.IsShift()) ? arg2.AsUnsigned() : arg2,
                    Operator);

        public override bool IsEqualTo(Instruction other)
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