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

            if (@operator == BinaryOperator.Default)
            {
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

        internal override Expression ReplaceCore(Expression currentExpression, Expression newExpression, bool replaceAll, bool allowConditional)
        {
            if (IsEquivalentTo(currentExpression))
            {
                return newExpression;
            }

            var l = Left.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional);

            if (!allowConditional && l == Left)
            {
                // TODO: LogicalOperator
            }

            var r = replaceAll || l == Left ? Right.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional) : Right;

            return l == Left && r == Right ? this : new BinaryExpression(l, r, Operator);
        }
    }
}