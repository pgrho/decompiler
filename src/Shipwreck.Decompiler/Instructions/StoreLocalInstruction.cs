using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class StoreLocalInstruction : StoreIndexInstruction
    {
        public StoreLocalInstruction(int index)
            : base(index)
        {
        }

        internal override Expression CreateStoreExpression(DecompilationContext context, Expression value)
            => context.GetParameter(context.Method.IsStatic ? Index : (Index - 1)).Assign(value);

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is StoreLocalInstruction sl && Index == sl.Index);

        public override string ToString()
            => $"stloc {Index}";
    }
}