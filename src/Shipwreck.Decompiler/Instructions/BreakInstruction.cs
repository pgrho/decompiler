using System.Diagnostics;
using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class BreakInstruction : Instruction
    {
        public static readonly BreakInstruction Default = new BreakInstruction();

        public override FlowControl FlowControl
            => FlowControl.Next;

        public override int PopCount
            => 0;

        public override int PushCount
            => 0;

        public ushort OpCode
            => 0x01;

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = null;
            return false;
        }

        internal override bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            statement = new MethodCallExpression(typeof(Debugger).GetMethod(nameof(Debugger.Break))).ToStatement();
            return false;
        }

        public override bool IsEqualTo(Syntax other)
            => this == other || other is BreakInstruction;

        public override string ToString()
            => "break";
    }
}