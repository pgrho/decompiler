using System;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadLengthInstruction : UnaryExpressionInstruction
    {
        internal override Expression CreateExpression(DecompilationContext context, Expression value)
            => value.Property(typeof(Array).GetProperty(nameof(Array.Length)));

        public override bool IsEqualTo(Instruction other)
            => this == other
            || other is LoadLengthInstruction;

        public override string ToString()
            => "ldlen";
    }
}