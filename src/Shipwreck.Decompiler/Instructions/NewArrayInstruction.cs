using System;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class NewArrayInstruction : UnaryExpressionInstruction
    {
        public NewArrayInstruction(Type type)
        {
            type.ArgumentIsNotNull(nameof(type));

            Type = type;
        }

        public Type Type { get; }

        internal override Expression CreateExpression(DecompilationContext context, Expression value)
            => new NewArrayExpression(Type, value);

        public override bool IsEquivalentTo(Syntax other)
            => this == other
            || (other is NewArrayInstruction ui && Type == ui.Type);

        public override string ToString()
            => "newarr " + Type.FullName;
    }
}