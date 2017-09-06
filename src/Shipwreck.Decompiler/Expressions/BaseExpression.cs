using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class BaseExpression : Expression
    {
        internal BaseExpression(Type type)
        {
            type.ArgumentIsNotNull(nameof(type));

            Type = type;
        }

        public override Type Type { get; }

        public override bool IsEquivalentTo(Syntax other)
            => this == other
            || (other is BaseExpression te && te.Type == Type);

        public override void WriteTo(TextWriter writer)
            => writer.Write("base");

        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Primary;
    }
}