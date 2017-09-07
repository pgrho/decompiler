using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shipwreck.CSharpModels.Expressions
{
    public abstract class Expression : Syntax
    {
        public abstract ExpressionPrecedence Precedence { get; }

        public abstract Type Type { get; }

        public void WriteTo(TextWriter writer) 
            => AcceptVisitor(CSharpSyntaxWriter.Default, writer);

        public override string ToString()
        {
            using (var sw = new StringWriter())
            {
                WriteTo(sw);
                return sw.ToString();
            }
        }

        #region Reduce

        public Expression Reduce()
        {
            var r = ReduceCore();
            return r == this ? this : r.Reduce();
        }

        public bool TryReduce(out Expression expression)
        {
            expression = Reduce();
            return expression != this;
        }

        internal virtual Expression ReduceCore()
            => this;

        #endregion Reduce

        #region Replace

        public bool TryReplace(Expression currentExpression, Expression newExpression, out Expression result)
            => (result = ReplaceCore(currentExpression, newExpression, false, false)) != this;

        internal virtual Expression ReplaceCore(Expression currentExpression, Expression newExpression, bool replaceAll, bool allowConditional)
            => IsEqualTo(currentExpression) ? newExpression : this;

        #endregion Replace

        public virtual IEnumerable<Expression> GetChildren()
            => Enumerable.Empty<Expression>();

        public IEnumerable<Expression> EnumeratePostOrder()
        {
            var iters = new Stack<IEnumerator<Expression>>();
            var nodes = new Stack<Expression>();
            iters.Push(GetChildren().GetEnumerator());
            nodes.Push(this);

            while (iters.Any())
            {
                var iter = iters.Peek();

                if (iter.MoveNext())
                {
                    iters.Push(iter.Current.GetChildren().GetEnumerator());
                    nodes.Push(iter.Current);
                }
                else
                {
                    yield return nodes.Pop();
                    iters.Pop();
                }
            }
        }

        #region AcceptVisitor

        public abstract void AcceptVisitor(IExpressionVisitor visitor);

        public abstract TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor);

        public abstract void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter);

        public abstract TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter);

        #endregion AcceptVisitor
    }
}