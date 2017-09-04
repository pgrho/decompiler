namespace Shipwreck.Decompiler.Expressions
{
    public enum BinaryOperator
    {
        Default,

        Add,
        AddChecked,
        Subtract,
        SubtractChecked,
        Multiply,
        MultiplyChecked,
        Divide,
        Modulo,

        And,
        Or,
        ExclusiveOr,
        LeftShift,
        RightShift,

        AndAlso,
        OrElse,

        Equal,
        NotEqual,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
    }
}