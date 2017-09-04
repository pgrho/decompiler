using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class BranchInstruction : BranchInstructionBase
    {
        public BranchInstruction(int target, bool? branchWhen = null)
            : base(target)
        {
            BranchWhen = branchWhen;
        }

        public bool? BranchWhen { get; }

        public override int PopCount
            => BranchWhen != null ? 1 : 0;

        internal override bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            if (BranchWhen == null)
            {
                return base.TryCreateStatement(context, ref startIndex, ref lastIndex, out statement);
            }
            else
            {
                if (context.GetFromCount(this) <= 1 && startIndex >= 1)
                {
                    var j = startIndex - 1;

                    if (context.TryCreateExpression(ref j, out var e))
                    {
                        startIndex = j;

                        if (BranchWhen == false)
                        {
                            e = e.OnesComplement();
                        }

                        var ib = new IfStatement(e);
                        ib.TruePart.Add(new TemporalGoToStatement(Target));
                        statement = ib;

                        return true;
                    }
                }
                statement = null;
                return false;
            }
        }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is BranchInstruction bs && BranchWhen == bs.BranchWhen && Target == bs.Target);

        public override string ToString()
            => BranchWhen == null ? $"br L_{Target:x4}"
                : BranchWhen == true ? $"br.true L_{Target:x4}"
                : $"br.false L_{Target:x4}";

        internal override void SetTo(DecompilationContext context, int index)
        {
            if (BranchWhen != null)
            {
                context.SetTo(this, new[] { context.RootStatements[index + 1], context.GetSyntaxAt(Target) });
            }
            else
            {
                base.SetTo(context, index);
            }
        }
    }
}