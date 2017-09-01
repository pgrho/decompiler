using System;
using System.Reflection;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class ParameterExpression : Expression
    {
        public ParameterExpression(string name, Type type)
        {
            Name = name;
            Type = type;
        }

        internal ParameterExpression(ParameterInfo parameter)
            : this(parameter.Name, parameter.ParameterType)
        {
        }

        public string Name { get; }
        public Type Type { get; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
                || (other is ParameterExpression pe && Name == pe.Name && Type == pe.Type);
    }
}