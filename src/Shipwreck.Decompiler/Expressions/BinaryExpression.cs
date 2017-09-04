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

            writer.WriteFirstChild(Left, this);

            writer.Write(' ');
            writer.Write(Operator.GetToken());
            writer.Write(' ');

            writer.WriteSecondChild(Right, this);

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

        public override ExpressionPrecedence Precedence
        {
            get
            {
                switch (Operator)
                {
                    case BinaryOperator.AddChecked:
                    case BinaryOperator.SubtractChecked:
                    case BinaryOperator.MultiplyChecked:
                        return ExpressionPrecedence.Primary;

                    case BinaryOperator.Multiply:
                    case BinaryOperator.Divide:
                    case BinaryOperator.Modulo:
                        return ExpressionPrecedence.Multiplicative;

                    case BinaryOperator.Add:
                    case BinaryOperator.Subtract:
                        return ExpressionPrecedence.Additive;

                    case BinaryOperator.LeftShift:
                    case BinaryOperator.RightShift:
                        return ExpressionPrecedence.Shift;

                    case BinaryOperator.GreaterThan:
                    case BinaryOperator.GreaterThanOrEqual:
                    case BinaryOperator.LessThan:
                    case BinaryOperator.LessThanOrEqual:
                        // TODO:is
                        return ExpressionPrecedence.Relational;

                    case BinaryOperator.Equal:
                    case BinaryOperator.NotEqual:
                        return ExpressionPrecedence.Equality;

                    case BinaryOperator.And:
                        return ExpressionPrecedence.And;

                    case BinaryOperator.ExclusiveOr:
                        return ExpressionPrecedence.ExclusiveOr;

                    case BinaryOperator.Or:
                        return ExpressionPrecedence.Or;


                    case BinaryOperator.AndAlso:
                        return ExpressionPrecedence.AndAlso;

                    case BinaryOperator.OrElse:
                        return ExpressionPrecedence.OrElse;
                }

                return ExpressionPrecedence.Primary;
            }
        }
    }
}