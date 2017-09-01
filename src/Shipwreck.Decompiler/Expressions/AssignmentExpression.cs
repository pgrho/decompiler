namespace Shipwreck.Decompiler.Expressions
{
    public sealed class AssignmentExpression : Expression
    {
        public AssignmentExpression(Expression left, Expression right, BinaryOperator @operator = BinaryOperator.Default)
        {
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
    }
}