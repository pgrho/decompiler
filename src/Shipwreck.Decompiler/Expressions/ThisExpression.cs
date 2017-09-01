using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class ThisExpression : Expression
    {
        public override bool IsEquivalentTo(Syntax other)
            => other is ThisExpression;

        public override void WriteTo(TextWriter writer)
            // "base" should be written by member access expressions.
            => writer.Write("this");
    }
}