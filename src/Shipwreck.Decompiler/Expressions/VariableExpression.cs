using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class VariableExpression : Expression
    {
        public VariableExpression(int index)
        {
            Index = index;
        }

        public int Index { get; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
                || (other is VariableExpression ve && Index == ve.Index);

        public override void WriteTo(TextWriter writer)
        {
            writer.Write("$$arg");
            writer.Write(Index);
        }
    }
}