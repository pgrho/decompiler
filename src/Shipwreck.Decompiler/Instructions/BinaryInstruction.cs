using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class BinaryInstruction : Instruction
    {
        public BinaryInstruction(BinaryOperator @operator, bool unsigned = false)
        {
            Operator = @operator;
            IsUnsigned = unsigned;
        }

        public BinaryOperator Operator { get; }

        public bool IsUnsigned { get; }

        public override FlowControl FlowControl
            => FlowControl.Next;

        public override int PopCount
            => 2;

        public override int PushCount
            => 1;

        internal override bool TryCreateExpression(MethodBase method, List<Syntax> list, ref int index, out Expression expression)
        {
            if (index > 1)
            {
                var j = index - 1;
                var prev = list[j] as Instruction;
                if (prev != null && prev.TryCreateExpression(method, list, ref j, out var right))
                {
                    j--;
                    if (j >= 0)
                    {
                        prev = list[j] as Instruction;
                        if (prev != null && prev.TryCreateExpression(method, list, ref j, out var left))
                        {
                            index = j;
                            expression = left.MakeBinary(right, Operator);
                            return true;
                        }
                    }
                }
            }
            expression = null;
            return false;
        }

        internal override bool TryCreateStatement(MethodBase method, List<Syntax> list, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            statement = null;
            return false;
        }

        public override bool IsEquivalentTo(Syntax other)
            => other is BinaryInstruction bi
                && Operator == bi.Operator
                && IsUnsigned == bi.IsUnsigned;
    }
}