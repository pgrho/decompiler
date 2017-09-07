using System;

namespace Shipwreck.CSharpModels.Expressions
{
    public sealed partial class ConstantExpression : Expression
    {
        internal ConstantExpression(object value, Type type = null)
        {
            Value = value;
            Type = type ?? value?.GetType() ?? typeof(object);
        }

        public object Value { get; }

        public override Type Type { get; }

        public override bool IsEqualTo(Syntax other)
            => other is ConstantExpression ce && Equals(Value, ce.Value);

        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Primary;
    }
}