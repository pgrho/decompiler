using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class IgnorableInstruction : Instruction, IIgnorableInstruction
    {
        private IgnorableInstruction(ushort opCode, string name)
        {
            OpCode = opCode;
            Name = name;
        }

        #region Known Ignorable OpCodes

        public static readonly IgnorableInstruction Nop = new IgnorableInstruction(0x00, "nop");
        public static readonly IgnorableInstruction EndFinally = new IgnorableInstruction(0xdc, "endfinally");

        #endregion Known Ignorable OpCodes

        public ushort OpCode { get; }

        public string Name { get; }

        public override FlowControl FlowControl => FlowControl.Next;

        public override int PopCount => 0;

        public override int PushCount => 0;

        public override string ToString()
            => Name;

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = null;
            return false;
        }

        internal override bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            statement = null;
            return true;
        }

        public override bool IsEqualTo(Instruction other)
            => this == other;
    }
}