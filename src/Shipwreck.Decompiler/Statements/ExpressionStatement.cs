using System.CodeDom.Compiler;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class ExpressionStatement : Statement
    {
        public ExpressionStatement(Expression expression)
        {
            Expression = expression;
        }

        public Expression Expression { get; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is ExpressionStatement es && (Expression?.IsEquivalentTo(es?.Expression) == true));

        public override void WriteTo(IndentedTextWriter writer)
        {
            Expression.WriteTo(writer);
            writer.WriteLine(';');
        }
    }
}