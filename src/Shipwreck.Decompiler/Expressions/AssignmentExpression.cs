using System;
using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class AssignmentExpression : Expression
    {
        public AssignmentExpression(Expression left, Expression right, BinaryOperator @operator = BinaryOperator.Default)
        {
            left.ArgumentIsNotNull(nameof(left));
            right.ArgumentIsNotNull(nameof(right));

            switch (@operator)
            {
                case BinaryOperator.Default:
                case BinaryOperator.Add:
                case BinaryOperator.AddChecked:
                case BinaryOperator.Subtract:
                case BinaryOperator.SubtractChecked:
                case BinaryOperator.Multiply:
                case BinaryOperator.MultiplyChecked:
                case BinaryOperator.Divide:
                    break;

                default:
                    throw new ArgumentException($"Unsupported {nameof(@operator)}");
            }

            Left = left;
            Right = right;
            Operator = Operator;
        }

        public Expression Left { get; }
        public Expression Right { get; }
        public BinaryOperator Operator { get; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
                || (other is AssignmentExpression ae
                    && Left.IsEquivalentTo(ae.Left)
                    && Right.IsEquivalentTo(ae.Right)
                    && Operator == ae.Operator);

        public override void WriteTo(TextWriter writer)
        {
            var isChecked = Operator.IsChecked();

            if (isChecked)
            {
                writer.Write("checked(");
            }

            Left.WriteTo(writer);

            writer.Write(' ');
            if (Operator != BinaryOperator.Default)
            {
                writer.Write(Operator.GetToken());
            }
            writer.Write("= ");

            writer.Write('(');
            Right.WriteTo(writer);
            writer.Write(')');

            if (isChecked)
            {
                writer.Write(")");
            }
        }
    }
}