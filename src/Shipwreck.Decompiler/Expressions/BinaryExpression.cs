using System;
using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class BinaryExpression : Expression
    {
        public BinaryExpression(Expression left, Expression right, BinaryOperator @operator)
        {
            left.ArgumentIsNotNull(nameof(left));
            right.ArgumentIsNotNull(nameof(right));

            switch (@operator)
            {
                case BinaryOperator.Add:
                case BinaryOperator.AddChecked:
                case BinaryOperator.Subtract:
                case BinaryOperator.SubtractChecked:
                case BinaryOperator.Multiply:
                case BinaryOperator.MultiplyChecked:
                case BinaryOperator.Divide:
                case BinaryOperator.Equal:
                case BinaryOperator.NotEqual:
                case BinaryOperator.LessThan:
                case BinaryOperator.LessThanOrEqual:
                case BinaryOperator.GreaterThan:
                case BinaryOperator.GreaterThanOrEqual:
                    break;

                default:
                    throw new ArgumentException($"Unsupported {nameof(@operator)}");
            }

            Left = left;
            Right = right;
            Operator = @operator;
        }

        public Expression Left { get; }
        public Expression Right { get; }
        public BinaryOperator Operator { get; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
                || (other is BinaryExpression be
                    && Left.IsEquivalentTo(be.Left)
                    && Right.IsEquivalentTo(be.Right)
                    && Operator == be.Operator);

        public override void WriteTo(TextWriter writer)
        {
            var isChecked = Operator.IsChecked();

            if (isChecked)
            {
                writer.Write("checked(");
            }

            writer.Write('(');
            Left.WriteTo(writer);
            writer.Write(')');

            writer.Write(' ');
            writer.Write(Operator.GetToken());
            writer.Write(' ');

            writer.Write('(');
            Right.WriteTo(writer);
            writer.Write(')');

            if (isChecked)
            {
                writer.Write(')');
            }
        }

        internal override Expression ReduceCore()
        {
            if (Left.TryReduce(out var l) | Right.TryReduce(out var r))
            {
                return new BinaryExpression(l, r, Operator);
            }

            return base.ReduceCore();
        }
    }
}