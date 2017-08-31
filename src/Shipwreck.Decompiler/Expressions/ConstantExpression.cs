namespace Shipwreck.Decompiler.Expressions
{
    public sealed class ConstantExpression : Expression
    {
        public ConstantExpression(object value)
        {
            Value = value;
        }

        public object Value { get; }

        public override bool IsEquivalentTo(Syntax other)
            => other is ConstantExpression ce && Equals(Value, ce.Value);
    }
}