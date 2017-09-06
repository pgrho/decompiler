using System.CodeDom.Compiler;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class ThrowStatement : Statement, IBreakingStatement
    {
        public ThrowStatement()
        {
        }

        public ThrowStatement(Expression value)
        {
            Value = value;
        }

        public Expression Value { get; set; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is ThrowStatement rs && (Value?.IsEquivalentTo(rs?.Value) ?? rs.Value == null));

        public override void WriteTo(IndentedTextWriter writer)
        {
            writer.Write("throw");
            if (Value != null)
            {
                writer.Write(' ');
                Value.WriteTo(writer);
            }
            writer.WriteLine(';');
        }

        public override bool Reduce()
        {
            var reduced = false;
            if (Value != null)
            {
                if (Value.TryReduce(out var e))
                {
                    Value = e;
                    reduced = true;
                }

                // TODO: check ref parameter
                if (Value is AssignmentExpression ae
                    && (ae.Left is VariableExpression || ae.Left is ParameterExpression))
                {
                    Value = ae.Right;
                    reduced = true;
                }
            }
            if (Collection != null)
            {
                var i = Collection.IndexOf(this);
                if (i > 0
                    && Value != null
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
            return reduced;
        }

        public override Statement Clone()
            => new ThrowStatement(Value);
    }
}