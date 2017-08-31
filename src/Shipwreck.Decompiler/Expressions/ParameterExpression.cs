namespace Shipwreck.Decompiler.Expressions
{
    public sealed class ParameterExpression : Expression
    {
        public ParameterExpression(int index)
        {
            Index = index;
        }

        public int Index { get; }

        public override bool IsEquivalentTo(Syntax other)
            => other is ParameterExpression ce && Index == ce.Index;
    }
}