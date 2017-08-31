using System.Collections.Generic;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    internal static class InstructionHelper
    {
        internal static Expression ProcessUnaryOperator(this Expression expression, List<Syntax> list, ref int index)
        {
            while (index > 0)
            {
                var prev = list[index];

                if (prev is NotInstruction)
                {
                    expression = expression.Not();
                    index--;
                    continue;
                }

                break;
            }

            return expression;
        }
    }
}