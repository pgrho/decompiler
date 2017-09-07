using System.IO;

namespace Shipwreck.CSharpModels.Expressions
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
        NullCoalescing,
        Conditional,
        Assignment
    }
}