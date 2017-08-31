namespace Shipwreck.Decompiler.Expressions
{
    public sealed class ThisExpression : Expression
    {
        public override bool IsEquivalentTo(Syntax other)
            => other is ThisExpression;
    }

}