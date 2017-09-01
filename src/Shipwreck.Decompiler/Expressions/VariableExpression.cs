namespace Shipwreck.Decompiler.Expressions
{
    public sealed class VariableExpression : Expression
    {
        public VariableExpression(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
                || (other is VariableExpression ve && Value == ve.Value);
    }
}