using System.Reflection;
using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class ReturnInstruction : Instruction
    {
        public override FlowControl FlowControl
            => FlowControl.Return;

        public override int PopCount
            => 1;

        public override int PushCount
            => 0;

        public ushort OpCode => 0x2a;

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = null;
            return false;
        }

        internal override bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            if (context.Method is ConstructorInfo || (context.Method as MethodInfo)?.ReturnType == typeof(void))
            {
                statement = new ReturnStatement();
                return true;
            }

            if (context.GetFromCount(this) <= 1 && startIndex > 0)
            {
                var j = startIndex - 1;
                if (context.TryCreateExpression(ref j, out var e))
                {
                    startIndex = j;
                    statement = e.ToReturnStatement();
                    return true;
                }
            }
            statement = null;
            return false;
        }

        public override bool IsEqualTo(Instruction other)
            => this == other || other is ReturnInstruction;

        public override string ToString()
            => "ret";
    }
}