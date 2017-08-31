using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Expressions
{
    public static class ExpressionBuilder
    {
        public static ConstantExpression ToExpression(this object value)
            => new ConstantExpression(value);

        #region UnaryExpression

        public static UnaryExpression MakeUnary(this Expression operand, UnaryOperator @operator)
            => new UnaryExpression(operand, @operator);

        public static UnaryExpression Not(this Expression operand)
            => operand.MakeUnary(UnaryOperator.Not);

        public static UnaryExpression Negate(this Expression operand)
            => operand.MakeUnary(UnaryOperator.Negate);

        #endregion UnaryExpression

        public static ReturnStatement ToReturnStatement(this Expression value)
            => new ReturnStatement(value);
    }
}