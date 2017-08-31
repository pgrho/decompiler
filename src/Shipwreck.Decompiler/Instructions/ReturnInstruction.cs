using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class ReturnInstruction : Instruction
    {
        public ReturnInstruction(int offset)
            : base(offset)
        { }

        public override FlowControl FlowControl
            => FlowControl.Return;

        public override int PopCount
            => 1;

        public override int PushCount
            => 0;

        internal override bool TryCreateExpression(MethodBase method, List<Syntax> list, ref int index, out Expression expression)
        {
            expression = null;
            return false;
        }

        internal override bool TryCreateStatement(MethodBase method, List<Syntax> list, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            if (startIndex > 0)
            {
                var j = startIndex - 1;
                var prev = list[j] as Instruction;
                if (prev != null && prev.TryCreateExpression(method, list, ref j, out var e))
                {
                    startIndex = j;
                    statement = e.ToReturnStatement();
                    return true;
                }
            }
            statement = null;
            return false;
        }

        public override bool IsEquivalentTo(Syntax other)
            => other is ReturnInstruction;
    }
}