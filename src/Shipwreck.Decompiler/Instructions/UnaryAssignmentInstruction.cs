using System.Reflection.Emit;
using Shipwreck.CSharpModels.Expressions;
using Shipwreck.CSharpModels.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public abstract class UnaryAssignmentInstruction : Instruction
    {
        public override FlowControl FlowControl
       => FlowControl.Next;

        public override int PopCount
            => 1;

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
                        expression = CreateExpression(context, e);
                        if (expression != null)
                        {
                            index = j;
                            return true;
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
                    var assign = CreateExpression(context, e);
                    if (assign != null)
                    {
                        startIndex = j;
                        statement = assign.ToStatement();
                        return true;
                    }
                }
            }

            statement = null;
            return false;
        }

        internal abstract Expression CreateExpression(DecompilationContext context, Expression value);
    }
}