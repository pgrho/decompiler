using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public abstract class Instruction : Syntax
    {
        public abstract FlowControl FlowControl { get; }

        public abstract int PopCount { get; }

        public abstract int PushCount { get; }

        internal abstract bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression);

        internal abstract bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement);
    }
}