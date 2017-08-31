using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public abstract class Instruction : Syntax
    {
        internal Instruction(int offset)
        {
            Offset = offset;
        }

        public int Offset { get; }


        public abstract FlowControl FlowControl { get; }

        public abstract int PopCount { get; }

        public abstract int PushCount { get; }

        internal abstract bool TryCreateExpression(MethodBase method, List<Syntax> list, ref int index, out Expression expression);

        internal abstract bool TryCreateStatement(MethodBase method, List<Syntax> list, ref int startIndex, ref int lastIndex, out Statement statement);
    }

}