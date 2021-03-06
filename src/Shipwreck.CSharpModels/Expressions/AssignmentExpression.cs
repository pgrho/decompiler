using System;
using System.Collections.Generic;

namespace Shipwreck.CSharpModels.Expressions
{
    public sealed partial class AssignmentExpression : Expression
    {
        internal AssignmentExpression(Expression left, Expression right, BinaryOperator @operator = BinaryOperator.Default)
        {
            left.ArgumentIsNotNull(nameof(left));
            right.ArgumentIsNotNull(nameof(right));

            if (!@operator.CanAssign())
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

        public override Type Type => Left.Type;

        public override bool IsEqualTo(Syntax other)
            => this == (object)other
                || (other is AssignmentExpression ae
                    && Left.IsEqualTo(ae.Left)
                    && Right.IsEqualTo(ae.Right)
                    && Operator == ae.Operator);

        internal override Expression ReduceCore()
        {
            if (Operator == BinaryOperator.Default
                && Right is BinaryExpression b
                && b.Left.IsEqualTo(Left))
            {
                return Left.Assign(b.Right, b.Operator);
            }

            if (Left.TryReduce(out var l) | Right.TryReduce(out var r))
            {
                return new AssignmentExpression(l, r, Operator);
            }

            return this;
        }

        internal override Expression ReplaceCore(Expression currentExpression, Expression newExpression, bool replaceAll, bool allowConditional)
        {
            if (IsEqualTo(currentExpression))
            {
                return newExpression;
            }

            var r = Right.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional);
            var l = replaceAll || r == Right ? Left.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional) : Left;

            return l == Left && r == Right ? this : new AssignmentExpression(l, r, Operator);
        }

        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Assignment;

        public override IEnumerable<Expression> GetChildren()
        {
            yield return Right;
            yield return Left;
        }
    }
}