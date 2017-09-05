using System;
using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class ConvertInstruction : Instruction
    {
        public ConvertInstruction(Type type, bool isChecked, bool unsigned = false)
        {
            Type = type;
            IsChecked = isChecked;
            IsUnsigned = unsigned;
        }

        public Type Type { get; }

        public bool IsChecked { get; }

        public bool IsUnsigned { get; }

        public override FlowControl FlowControl
            => FlowControl.Next;

        public override int PopCount
            => 1;

        public override int PushCount
            => 1;

        public override bool IsEquivalentTo(Syntax other)
            => other is ConvertInstruction ci
                && Type == ci.Type
                && IsChecked == ci.IsChecked
                && IsUnsigned == ci.IsUnsigned;

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            if (context.GetFromCount(this) <= 1 && index > 0)
            {
                var j = index - 1;
                if (context.TryCreateExpression(ref j, out var e))
                {
                    index = j;
                    // TODO: make unsigned
                    expression = IsChecked ? e.ConvertChecked(Type) : e.Convert(Type);
                    return true;
                }
            }
            expression = null;
            return false;
        }

        internal override bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            statement = null;
            return false;
        }
    }
}