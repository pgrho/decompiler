using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class BaseExpression : Expression
    {
        public override bool IsEquivalentTo(Syntax other)
            => other is BaseExpression;

        public override void WriteTo(TextWriter writer)
            => writer.Write("base");

        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Primary;
    }
}