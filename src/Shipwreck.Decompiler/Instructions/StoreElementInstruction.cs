using System.Reflection.Emit;
using Shipwreck.CSharpModels.Expressions;
using Shipwreck.CSharpModels.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class StoreElementInstruction : Instruction
    {
        public override FlowControl FlowControl
            => FlowControl.Next;

        public override int PopCount
            => 3;

        public override int PushCount
            => 0;

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            // If previous instruction is dup

            if (context.GetFromCount(this) <= 1 && index >= 2)
            {
                if (context.RootStatements[index - 1] is DuplicateInstruction)
                {
                    var j = index - 2;

                    if (context.TryCreateExpression(ref j, out var e))
                    {
                        j--;
                        if (context.TryCreateExpression(ref j, out var ie))
                        {
                            j--;
                            if (context.TryCreateExpression(ref j, out var ae))
                            {
                                index = j;
                                expression = CreateStoreExpression(context, ae, ie, e);
                                return true;
                            }
                        }
                    }
                }
            }

            expression = null;
            return false;
        }

        internal override bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            // If previous instruction is not dup
            if (context.GetFromCount(this) <= 1 && startIndex >= 1)
            {
                var j = startIndex - 1;

                if (context.TryCreateExpression(ref j, out var e))
                {
                    j--;
                    if (context.TryCreateExpression(ref j, out var ie))
                    {
                        j--;
                        if (context.TryCreateExpression(ref j, out var ae))
                        {
                            startIndex = j;
                            statement = CreateStoreExpression(context, ae, ie, e).ToStatement();
                            return true;
                        }
                    }
                }
            }

            statement = null;
            return false;
        }

        private static Expression CreateStoreExpression(DecompilationContext context, Expression array, Expression index, Expression value)
            => array.MakeIndex(index).Assign(value);

        public override bool IsEqualTo(Instruction other)
            => this == other
            || other is StoreElementInstruction;

        public override string ToString()
            => "stelem";
    }
}