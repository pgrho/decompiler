namespace Shipwreck.Decompiler.Expressions
{
    public sealed class UnaryExpression : Expression
    {
        public Expression Operand { get; }

        public UnaryOperator Operator { get; }

        public UnaryExpression(Expression operand, UnaryOperator @operator)
        {
            Operand = operand;
            Operator = @operator;
        }

        public override bool IsEquivalentTo(Syntax other)
            => other is UnaryExpression ue
                && Operand.IsEquivalentTo(ue.Operand)
                && Operator == ue.Operator;
    }
}