using Shipwreck.Decompiler.Expressions;

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
            expression = new VariableExpression(Index);
            return true;
        }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other 
            || (other is LoadLocalInstruction li && Index == li.Index);

        public override string ToString()
            => $"ldloc {Index}";
    }
}