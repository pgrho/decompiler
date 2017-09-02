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
            var to = context.GetTo(this).FirstOrDefault();
            var toi = context.RootStatements.IndexOf(to);

            // Remove br if the target is next to this instance.
            if (toi == startIndex + 1)
            {
                statement = to as Statement;
                if (statement != null)
                {
                    lastIndex = toi;
                    return true;
                }
            }

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
            context.SetTo(this, context.GetSyntaxAt(Offset));
        }
    }
}