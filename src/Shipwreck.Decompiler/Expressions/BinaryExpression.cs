namespace Shipwreck.Decompiler.Expressions
{
    public sealed class BinaryExpression : Expression
    {
        public BinaryExpression(Expression left, Expression right, BinaryOperator @operator)
        {
            Left = left;
            Right = right;
            Operator = Operator;
        }

        public Expression Left { get; }
        public Expression Right { get; }
        public BinaryOperator Operator { get; }

        public override bool IsEquivalentTo(Syntax other)
            => other is BinaryExpression be
                && Left.IsEquivalentTo(be.Left)
                && Right.IsEquivalentTo(be.Right)
                && Operator == be.Operator;
    }
}