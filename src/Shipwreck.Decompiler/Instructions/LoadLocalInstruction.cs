using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadLocalInstruction : LoadIndexInstruction
    {
        public LoadLocalInstruction(int index)
            : base(index)
        {
        }

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = new VariableExpression(Index, context.Method.GetMethodBody().LocalVariables[Index].LocalType);

            // try to include previous assignments

            var pi = index - 1;
            if (context.TryCreatenStatement(ref pi, out var prev))
            {
                var e = (prev as ExpressionStatement)?.Expression;
                while (e is AssignmentExpression ae)
                {
                    if (ae.Left.IsEquivalentTo(expression))
                    {
                        expression = (prev as ExpressionStatement).Expression;
                        index = pi;
                        return true;
                    }

                    e = ae.Right;
                }
            }

            return true;
        }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is LoadLocalInstruction li && Index == li.Index);

        public override string ToString()
            => $"ldloc {Index}";
    }
}