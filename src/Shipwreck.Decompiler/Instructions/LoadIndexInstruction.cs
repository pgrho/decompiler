using System.Reflection.Emit;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public abstract class LoadIndexInstruction : Instruction
    {
        public LoadIndexInstruction(int index)
        {
            Index = index;
        }

        public int Index { get; }

        public override FlowControl FlowControl
            => FlowControl.Next;

        public override int PopCount
            => 0;

        public override int PushCount
            => 1;

        internal override bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            statement = null;
            return false;
        }
    }
}