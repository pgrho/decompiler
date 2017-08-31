using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Expressions
{
    public static class ExpressionBuilder
    {
        public static ConstantExpression ToExpression(this object value)
            => new ConstantExpression(value);

        public static ReturnStatement ToReturnStatement(this Expression value)
            => new ReturnStatement(value);
    }
}