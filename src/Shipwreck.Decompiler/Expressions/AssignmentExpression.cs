using System;
using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class AssignmentExpression : Expression
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

            writer.WriteSecondChild(Left, this);

            writer.Write(' ');
            if (Operator != BinaryOperator.Default)
            {
                writer.Write(Operator.GetToken());
            }
            writer.Write("= ");

            writer.WriteFirstChild(Right, this);

            if (isChecked)
            {
                writer.Write(")");
            }
        }

        internal override Expression ReduceCore()
        {
            if (Operator == BinaryOperator.Default
                && Right is BinaryExpression b
                && b.Left.IsEquivalentTo(Left))
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
            if (IsEquivalentTo(currentExpression))
            {
                return newExpression;
            }

            var r = Right.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional);
            var l = replaceAll || r == Right ? Left.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional) : Left;

            return l == Left && r == Right ? this : new AssignmentExpression(l, r, Operator);
        }

        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Assignment;
    }
}