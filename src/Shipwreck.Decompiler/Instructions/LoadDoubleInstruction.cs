using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadDoubleInstruction : Instruction
    {
        public LoadDoubleInstruction(double value)
        {
            Value = value;
        }

        public double Value { get; }

        public override FlowControl FlowControl
            => FlowControl.Next;

        public override int PopCount
            => 0;

        public override int PushCount
            => 1;

        internal override bool TryCreateExpression(MethodBase method, List<Syntax> list, ref int index, out Expression expression)
        {
            expression = new ConstantExpression(Value).ProcessUnaryOperator(list, ref index);

            return true;
        }

        internal override bool TryCreateStatement(MethodBase method, List<Syntax> list, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            statement = null;
            return false;
        }

        public override bool IsEquivalentTo(Syntax other)
            => other is LoadDoubleInstruction li && Value == li.Value;
    }
}