using System.Linq;
using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class BranchInstruction : Instruction
    {
        public BranchInstruction(int offset, bool? branchWhen = null)
        {
            Offset = offset;
            BranchWhen = branchWhen;
        }

        public bool? BranchWhen { get; }

        /// <summary>
        /// Gets the absolute bytes offset from the beginning of the IL where instruction will transfer the control to.
        /// </summary>
        public int Offset { get; }

        public override FlowControl FlowControl
            => FlowControl.Branch;

        public override int PopCount
            => BranchWhen != null ? 1 : 0;

        public override int PushCount
            => 0;

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = null;
            return false;
        }

        internal override bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            if (BranchWhen == null)
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

                statement = new TemporalGoToStatement(Offset);
                return true;
            }
            else
            {
                if (context.GetFromCount(this) <= 1 && startIndex >= 1)
                {
                    var j = startIndex - 1;

                    var prev = context.RootStatements[j] as Instruction;
                    if (prev != null && prev.TryCreateExpression(context, ref j, out var e))
                    {
                        startIndex = j;

                        if (BranchWhen == false)
                        {
                            e = e.Negate();
                        }

                        var ib = new IfBlock(e);
                        ib.TruePart.Add(new TemporalGoToStatement(Offset));
                        statement = ib;

                        return true;
                    }
                }
            }

            statement = null;
            return false;
        }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is BranchInstruction bs && BranchWhen == bs.BranchWhen && Offset == bs.Offset);

        public override string ToString()
            => BranchWhen == null ? $"br L_{Offset:x4}"
                : BranchWhen == true ? $"br.true L_{Offset:x4}"
                : $"br.false L_{Offset:x4}";

        internal override void SetTo(DecompilationContext context, int index)
        {
            if (BranchWhen != null)
            {
                context.SetTo(this, new[] { context.RootStatements[index + 1], context.GetSyntaxAt(Offset) });
            }
            else
            {
                context.SetTo(this, context.GetSyntaxAt(Offset));
            }
        }
    }
}