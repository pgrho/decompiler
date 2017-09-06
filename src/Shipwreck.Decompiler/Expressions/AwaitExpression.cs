using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class AwaitExpression : Expression
    {
        public AwaitExpression(Expression expression)
        {
            expression.ArgumentIsNotNull(nameof(expression));

            Expression = expression;
        }

        public Expression Expression { get; }

        public override Type Type
        {
            get
            {
                var t = Expression.Type;
                if (t == typeof(Task)
                    || t == typeof(ConfiguredTaskAwaitable))
                {
                    return typeof(void);
                }
                else if (t.IsConstructedGenericType
                    && (t.GetGenericTypeDefinition() == typeof(Task<>) || t.GetGenericTypeDefinition() == typeof(TaskAwaiter<>)))
                {
                    return t.GetGenericArguments()[0];
                }

                var ga = t.GetMethod(nameof(Task.GetAwaiter), Type.EmptyTypes);
                if (ga != null)
                {
                    var gr = ga.ReturnType.GetMethod(nameof(TaskAwaiter<int>.GetResult), Type.EmptyTypes);
                    return gr?.ReturnType ?? typeof(void);
                }
                return t;
            }
        }

        public override bool IsEquivalentTo(Syntax other)
            => other is AwaitExpression ae
                && Expression.IsEquivalentTo(ae.Expression);

        public override void WriteTo(TextWriter writer)
        {
            writer.Write("await ");
            writer.WriteFirstChild(Expression, this);
        }

        internal override Expression ReduceCore()
            => Expression.TryReduce(out var v) ? new AwaitExpression(v) : this;

        internal override Expression ReplaceCore(Expression currentExpression, Expression newExpression, bool replaceAll, bool allowConditional)
        {
            if (IsEquivalentTo(currentExpression))
            {
                return newExpression;
            }

            var v = Expression.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional);

            return v == Expression ? this : new AwaitExpression(v);
        }

        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Assignment;

        public override IEnumerable<Expression> GetChildren()
        {
            yield return Expression;
        }
    }
}