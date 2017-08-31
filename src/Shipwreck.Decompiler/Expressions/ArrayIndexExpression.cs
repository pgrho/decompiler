namespace Shipwreck.Decompiler.Expressions
{
    public sealed class ArrayIndexExpression : Expression
    {
        public ArrayIndexExpression(Expression array, Expression index)
        {
            Array = array;
            Index = index;
        }

        public Expression Array { get; }
        public Expression Index { get; }

        public override bool IsEquivalentTo(Syntax other)
            => other is ArrayIndexExpression aie
                && Array.IsEquivalentTo(aie.Array)
                && Index.IsEquivalentTo(aie.Index);
    }
}