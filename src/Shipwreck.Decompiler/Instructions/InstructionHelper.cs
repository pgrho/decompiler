using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    internal static class InstructionHelper
    {
        public static bool TryCreateExpression(this DecompilationContext context, ref int index, out Expression expression)
        {
            var j = index;
            if (0 <= j && j < context.RootStatements.Count)
            {
                var s = context.RootStatements[j];

                if (s is Instruction il)
                {
                    if (il.TryCreateExpression(context, ref j, out expression))
                    {
                        index = j;
                        return true;
                    }
                }
                else if (s is ExpressionStatement es)
                {
                    expression = es.Expression;
                    return true;
                }
            }
            expression = null;
            return false;
        }

        public static bool TryCreatenStatement(this DecompilationContext context, ref int index, out Statement statement)
        {
            var j = index;
            if (0 <= j && j < context.RootStatements.Count)
            {
                var s = context.RootStatements[j];
                var k = index;

                if (s is Instruction il)
                {
                    if (il.TryCreateStatement(context, ref j, ref k, out statement)
                        && k == index)
                    {
                        index = j;
                        return true;
                    }
                }
                else if (s is Statement es)
                {
                    statement = es;
                    return true;
                }
            }
            statement = null;
            return false;
        }
    }
}