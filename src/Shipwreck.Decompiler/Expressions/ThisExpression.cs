using System;
using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class ThisExpression : Expression
    {
        public ThisExpression(Type type)
        {
            type.ArgumentIsNotNull(nameof(type));

            Type = type;
        }

        public override Type Type { get; }

        public override bool IsEquivalentTo(Syntax other)
            => this == other
            || (other is ThisExpression te && te.Type == Type);

        public override void WriteTo(TextWriter writer)
            => writer.Write("this");

        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Primary;
    }
}