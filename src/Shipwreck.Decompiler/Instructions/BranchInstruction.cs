using System.Linq;
using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class BranchInstruction : Instruction
    {
        public BranchInstruction(int offset)
        {
            Offset = offset;
        }

        /// <summary>
        /// Gets the absolute bytes offset from the beginning of the IL where instruction will transfer the control to.
        /// </summary>
        public int Offset { get; }

        public override FlowControl FlowControl
            => FlowControl.Branch;

        public override int PopCount
            => 0;

        public override int PushCount
            => 0;

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = null;
            return false;
        }

        internal override bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            statement = null;
            return false;
        }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is BranchInstruction sl && Offset == sl.Offset);

        public override string ToString()
            => $"br L_{Offset:x4}";

        internal override void SetTo(DecompilationContext context, int index)
        {
            context.Flow[index].SetTo(context.Flow.FirstOrDefault(t => t.Offset >= Offset));
        }
    }
}