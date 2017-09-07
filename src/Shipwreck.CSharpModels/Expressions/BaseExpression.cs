using System;

namespace Shipwreck.CSharpModels.Expressions
{
    public sealed partial class BaseExpression : Expression
    {
        public BaseExpression(Type type)
        {
            type.ArgumentIsNotNull(nameof(type));

            Type = type;
        }

        public override Type Type { get; }

        public override bool IsEqualTo(Syntax other)
            => this == other
            || (other is BaseExpression te && te.Type == Type);

        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Primary;
    }
}