using System.Collections.Generic;
using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class IndexExpression : CallExpression
    {
        public IndexExpression(Expression @object, params Expression[] parameters)
            : base(parameters)
        {
            @object.ArgumentIsNotNull(nameof(@object));

            Object = @object;
        }

        public IndexExpression(Expression @object, IEnumerable<Expression> parameters)
            : base(parameters)
        {
            @object.ArgumentIsNotNull(nameof(@object));

            Object = @object;
        }

        internal IndexExpression(Expression @object, IEnumerable<Expression> parameters, bool shouldCopy)
            : base(parameters, shouldCopy)
        {
            @object.ArgumentIsNotNull(nameof(@object));

            Object = @object;
        }

        public Expression Object { get; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is IndexExpression ie
                    && Object.IsEquivalentTo(ie.Object)
                    && base.IsEquivalentTo(other));

        public override void WriteTo(TextWriter writer)
        {
            writer.WriteFirstChild(Object, this);
            writer.Write('[');
            WriteParametersTo(writer);
            writer.Write(']');
        }

        internal override Expression ReduceCore()
        {
            if (Object.TryReduce(out var a) | TryReduceParameters(out var parameters))
            {
                return new IndexExpression(a, parameters, false);
            }

            return base.ReduceCore();
        }

        internal override Expression ReplaceCore(Expression currentExpression, Expression newExpression, bool replaceAll, bool allowConditional)
        {
            if (IsEquivalentTo(currentExpression))
            {
                return newExpression;
            }

            var a = Object.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional);

            Expression[] ps = null;
            if (replaceAll || a == Object)
            {
                TryReplaceParameters(currentExpression, newExpression, replaceAll, allowConditional, out ps);
            }

            return a == Object && ps == null ? this : new IndexExpression(a, (IEnumerable<Expression>)ps ?? Parameters, false);
        }
        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Primary;
    }
}