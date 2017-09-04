using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public enum ExpressionPrecedence
    {
        Primary,
        Unary,
        Multiplicative,
        Additive,
        Shift,
        Relational,
        Equality,
        And,
        ExclusiveOr,
        Or,
        AndAlso,
        OrElse,
        Conditional,
        Assignment
    }
}