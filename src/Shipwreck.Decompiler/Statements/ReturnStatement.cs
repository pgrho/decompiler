using System.CodeDom.Compiler;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class ReturnStatement : Statement
    {
        public ReturnStatement(Expression value)
        {
            Value = value;
        }

        public Expression Value { get; set; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is ReturnStatement rs && ((Value == null && rs.Value == null) || (Value?.IsEquivalentTo(rs?.Value) == true)));

        public override void WriteTo(IndentedTextWriter writer)
        {
            writer.Write("return");
            if (Value != null)
            {
                writer.Write(' ');
                Value.WriteTo(writer);
            }
            writer.WriteLine(';');
        }

        public override bool Reduce()
        {
            if (Value != null && Value.TryReduce(out var e))
            {
                Value = e;
                return true;
            }
            if (Collection != null)
            {
                var i = Collection.IndexOf(this);
                if (i > 0
                    && Collection[i - 1] is ExpressionStatement es
                    && es.Expression is AssignmentExpression ae
                    && Value.TryReplace(ae.Left, ae, out var replaced))
                {
                    Collection.RemoveAt(i - 1);
                    Value = replaced;

                    return true;
                }
                // TODO: split Expression statement and return statement in void method
            }
            return base.Reduce();
        }
    }
}