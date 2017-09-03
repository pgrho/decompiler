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
    }
}