using System.CodeDom.Compiler;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed partial class ExpressionStatement : Statement
    {
        public ExpressionStatement(Expression expression)
        {
            Expression = expression;
        }

        public Expression Expression { get; set; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is ExpressionStatement es && (Expression?.IsEquivalentTo(es?.Expression) == true));

        public override void WriteTo(IndentedTextWriter writer)
        {
            Expression.WriteTo(writer);
            writer.WriteLine(';');
        }

        public override bool Reduce()
        {
            if (Expression.TryReduce(out var e))
            {
                Expression = e;
                return true;
            }

            if (Collection != null)
            {
                var i = Collection.IndexOf(this);
                if (i > 0
                    && Collection[i - 1] is ExpressionStatement es
                    && es.Expression is AssignmentExpression ae
                    && Expression.TryReplace(ae.Left, ae, out var replaced))
                {
                    Collection.RemoveAt(i - 1);
                    Expression = replaced;

                    return true;
                }
            }

            return base.Reduce();
        }

        public override Statement Clone()
            => new ExpressionStatement(Expression);
    }
}