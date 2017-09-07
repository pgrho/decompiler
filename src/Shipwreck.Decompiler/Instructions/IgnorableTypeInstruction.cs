using System;
using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class IgnorableTypeInstruction : Instruction, IIgnorableInstruction
    {
        public IgnorableTypeInstruction(ushort opCode, string name, Type type)
        {
            OpCode = opCode;
            Name = name;
            Type = type;
        }

        public ushort OpCode { get; }

        public string Name { get; }

        public Type Type { get; }

        public override FlowControl FlowControl => FlowControl.Next;

        public override int PopCount => 0;

        public override int PushCount => 0;

        public override string ToString()
            => Name + " " + Type;

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
            => this == other
            || (other is IgnorableTypeInstruction iti && iti.OpCode == OpCode && iti.Type == Type);
    }
}