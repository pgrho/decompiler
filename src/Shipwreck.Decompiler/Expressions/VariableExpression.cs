using System;
using System.IO;

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

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
                || (other is VariableExpression ve && Index == ve.Index);

        public override void WriteTo(TextWriter writer)
        {
            writer.Write("$local");
            writer.Write(Index);
        }

        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Primary;
    }
}