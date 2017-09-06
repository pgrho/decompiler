using System;
using System.Collections.Generic;
using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed partial class TypeBinaryExpression : Expression
    {
        internal TypeBinaryExpression(Expression expression, Type type)
        {
            expression.ArgumentIsNotNull(nameof(expression));
            type.ArgumentIsNotNull(nameof(type));

            Expression = expression;
            TypeOperand = type;
        }

        public Expression Expression { get; }
        public Type TypeOperand { get; }

        public override Type Type
            => typeof(bool);

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
                || (other is TypeBinaryExpression be
                    && Expression.IsEquivalentTo(be.Expression)
                    && TypeOperand == TypeOperand);

        public override void WriteTo(TextWriter writer)
        {
            writer.WriteFirstChild(Expression, this);

            writer.Write(" is ");

            writer.Write(TypeOperand.FullName);
        }

        internal override Expression ReduceCore()
        {
            if (Expression.TryReduce(out var l))
            {
                return new TypeBinaryExpression(l, Type);
            }

            return base.ReduceCore();
        }

        internal override Expression ReplaceCore(Expression currentExpression, Expression newExpression, bool replaceAll, bool allowConditional)
        {
            if (IsEquivalentTo(currentExpression))
            {
                return newExpression;
            }

            var l = Expression.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional);

            return l == Expression ? this : new TypeBinaryExpression(l, Type);
        }

        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Relational;

        public override IEnumerable<Expression> GetChildren()
        {
            yield return Expression;
        }
    }
}