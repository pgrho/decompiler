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

        #region BinaryExpression

        public static BinaryExpression MakeBinary(this Expression left, Expression right, BinaryOperator @operator)
            => new BinaryExpression(left, right, @operator);

        public static BinaryExpression Add(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.Add);

        public static BinaryExpression AddChecked(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.AddChecked);

        public static BinaryExpression Subtract(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.Subtract);

        public static BinaryExpression SubtractChecked(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.SubtractChecked);

        public static BinaryExpression Multiply(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.Multiply);

        public static BinaryExpression MultiplyChecked(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.MultiplyChecked);

        public static BinaryExpression Divide(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.Divide);

        #endregion BinaryExpression

        #region AssignmentExpression

        public static AssignmentExpression Assign(this Expression left, Expression right, BinaryOperator @operator = BinaryOperator.Default)
            => new AssignmentExpression(left, right, @operator);

        public static AssignmentExpression AddAssign(this Expression left, Expression right)
            => left.Assign(right, BinaryOperator.Add);

        public static AssignmentExpression AddAssignChecked(this Expression left, Expression right)
            => left.Assign(right, BinaryOperator.AddChecked);

        public static AssignmentExpression SubtractAssign(this Expression left, Expression right)
            => left.Assign(right, BinaryOperator.Subtract);

        public static AssignmentExpression SubtractAssignChecked(this Expression left, Expression right)
            => left.Assign(right, BinaryOperator.SubtractChecked);

        public static AssignmentExpression MultiplyAssign(this Expression left, Expression right)
            => left.Assign(right, BinaryOperator.Multiply);

        public static AssignmentExpression MultiplyAssignChecked(this Expression left, Expression right)
            => left.Assign(right, BinaryOperator.MultiplyChecked);

        public static AssignmentExpression DivideAssign(this Expression left, Expression right)
            => left.Assign(right, BinaryOperator.Divide);

        #endregion AssignmentExpression

        public static ArrayIndexExpression ArrayIndex(this Expression array, Expression index)
            => new ArrayIndexExpression(array, index);

        public static ReturnStatement ToReturnStatement(this Expression value)
            => new ReturnStatement(value);

        public static ExpressionStatement ToStatement(this Expression value)
            => new ExpressionStatement(value);
    }
}