using System;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed partial class VariableExpression : Expression
    {
        public VariableExpression(int index, Type type)
        {
            type.ArgumentIsNotNull(nameof(type));

            Index = index;
            Type = type;
        }

        public override Type Type { get; }

        public int Index { get; }

        public override bool IsEqualTo(Syntax other)
            => this == (object)other
                || (other is VariableExpression ve && Index == ve.Index);

        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Primary;
    }
}