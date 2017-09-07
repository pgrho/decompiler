using System.Reflection.Emit;
using Shipwreck.CSharpModels.Expressions;
using Shipwreck.CSharpModels.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class DuplicateInstruction : Instruction
    {
        public override FlowControl FlowControl
        => FlowControl.Next;

        public override int PopCount
            => 0;

        public override int PushCount
            => 1;

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = null;
            return false;
        }

        internal override bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            var i = context.RootStatements.IndexOf(this);

            if (0 < i
                && i + 4 < context.RootStatements.Count
                && context.GetFromCount(this) <= 1)
            {
                if (context.RootStatements[i + 1] is BranchInstruction bi
                    && bi.BranchWhen == true
                    && context.GetOffset(i + 3) < bi.Target
                    && context.RootStatements[i + 2] is PopInstruction)
                {
                    var rightLast = context.RootStatements.IndexOf(context.GetSyntaxAt(bi.Target)) - 1;
                    var rightStart = rightLast;
                    if (context.TryCreateExpression(ref rightStart, out var right)
                        && rightStart == i + 3)
                    {
                        var leftStart = i - 1;
                        if (context.TryCreateExpression(ref leftStart, out var left))
                        {
                            startIndex = leftStart;
                            lastIndex = rightLast;
                            statement = left.NullCoalesce(right).ToStatement();
                            return true;
                        }
                    }
                }
            }

            statement = null;
            return false;
        }

        public override bool IsEqualTo(Instruction other)
            => this == (object)other || other is DuplicateInstruction;

        public override string ToString()
            => "dup";
    }
}