using System.Reflection.Emit;
using Shipwreck.CSharpModels.Expressions;
using Shipwreck.CSharpModels.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class PopInstruction : Instruction
    {
        public override FlowControl FlowControl
        => FlowControl.Next;

        public override int PopCount
            => 1;

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

        public override bool IsEqualTo(Instruction other)
            => this == (object)other || other is PopInstruction;

        public override string ToString()
            => "pop";
    }
}