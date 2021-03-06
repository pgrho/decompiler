using System;
using System.Reflection;

namespace Shipwreck.CSharpModels.Expressions
{
    public sealed partial class ParameterExpression : Expression
    {
        public ParameterExpression(string name, Type type)
        {
            name.ArgumentIsNotNull(nameof(name));
            type.ArgumentIsNotNull(nameof(type));

            Name = name;
            Type = type;
        }

        internal ParameterExpression(ParameterInfo parameter)
            : this(parameter.Name, parameter.ParameterType)
        {
        }

        public string Name { get; }
        public override Type Type { get; }

        public override bool IsEqualTo(Syntax other)
            => this == (object)other
                || (other is ParameterExpression pe && Name == pe.Name && Type == pe.Type);

        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Primary;
    }
}