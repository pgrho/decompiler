using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class ThisExpression : Expression
    {
        public override bool IsEquivalentTo(Syntax other)
            => other is ThisExpression;

        public override void WriteTo(TextWriter writer)
            => writer.Write("this");
        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Primary;
    }
}