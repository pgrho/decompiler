using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class StoreArgumentInstruction : UnaryAssignmentInstruction
    {
        public StoreArgumentInstruction(int index)
        {
            Index = index;
        }

        public int Index { get; }

        internal override Expression CreateExpression(DecompilationContext context, Expression value)
            => context.GetParameter(context.Method.IsStatic ? Index : (Index - 1)).Assign(value);

        public override bool IsEqualTo(Syntax other)
            => this == (object)other
            || (other is StoreArgumentInstruction sl && Index == sl.Index);

        public override string ToString()
            => $"starg {Index}";
    }
}