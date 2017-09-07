using System.Reflection.Emit;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LeaveInstruction : BranchInstructionBase
    {
        public LeaveInstruction(int target)
            : base(target)
        {
        }

        public override FlowControl FlowControl
            => FlowControl.Branch;

        public override int PopCount
            => 0;

        public override bool IsEqualTo(Instruction other)
            => this == (object)other
            || (other is LeaveInstruction li && Target == li.Target);

        public override string ToString()
            => $"leave L_{Target:x4}";
    }
}