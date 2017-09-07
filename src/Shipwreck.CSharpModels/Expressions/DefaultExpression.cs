using System;

namespace Shipwreck.CSharpModels.Expressions
{
    public sealed partial class DefaultExpression : Expression
    {
        public DefaultExpression(Type type)
        {
            type.ArgumentIsNotNull(nameof(type));
            Type = type;
        }

        public override Type Type { get; }

        public override bool IsEqualTo(Syntax other)
            => this == other
            || (other is DefaultExpression de && Type.IsEqualTo(de.Type));

        public override ExpressionPrecedence Precedence => ExpressionPrecedence.Primary;
    }
}