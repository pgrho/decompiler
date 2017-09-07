using System.Linq;
using System.Reflection.Emit;
using Shipwreck.CSharpModels.Expressions;
using Shipwreck.CSharpModels.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public abstract class BranchInstructionBase : Instruction
    {
        public BranchInstructionBase(int target)
        {
            Target = target;
        }

        public override int PushCount
            => 0;

        /// <summary>
        /// Gets the absolute bytes offset from the beginning of the IL where instruction will transfer the control to.
        /// </summary>
        public int Target { get; }

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = null;
            return false;
        }

        internal override bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            var to = context.GetTo(this).FirstOrDefault();
            var toi = context.RootStatements.IndexOf(to);

            // Remove br if the target is next to this instance.
            if (toi == startIndex + 1)
            {
                statement = to as Statement;
                if (statement != null)
                {
                    lastIndex = toi;
                    return true;
                }
            }

            statement = new TemporalGoToStatement(Target);
            return true;
        }

        internal override void SetTo(DecompilationContext context, int index)
            => context.SetTo(this, context.GetSyntaxAt(Target));
    }
}