using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class StoreLocalInstruction : UnaryAssignmentInstruction
    {
        public StoreLocalInstruction(int index)
        {
            Index = index;
        }

        public int Index { get; }

        internal override Expression CreateExpression(DecompilationContext context, Expression value)
                => new VariableExpression(Index, context.Method.GetMethodBody().LocalVariables[Index].LocalType).Assign(value);

        public override bool IsEqualTo(Instruction other)
            => this == (object)other
            || (other is StoreLocalInstruction sl && Index == sl.Index);

        public override string ToString()
            => $"stloc {Index}";
    }
}