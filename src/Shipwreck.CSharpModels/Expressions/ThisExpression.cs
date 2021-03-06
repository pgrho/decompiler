using System;

namespace Shipwreck.CSharpModels.Expressions
{
    public sealed partial class ThisExpression : Expression
    {
        public ThisExpression(Type type)
        {
            type.ArgumentIsNotNull(nameof(type));

            Type = type;
        }

        public override Type Type { get; }

        public override bool IsEqualTo(Syntax other)
            => this == other
            || (other is ThisExpression te && te.Type.IsEqualTo(Type));

        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Primary;
    }
}