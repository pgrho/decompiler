using System;
using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public abstract class Expression : Syntax
    {
        public abstract ExpressionPrecedence Precedence { get; }

        public abstract Type Type { get; }

        public abstract void WriteTo(TextWriter writer);

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
            => IsEquivalentTo(currentExpression) ? newExpression : this;

        #endregion Replace
    }
}