using System;
using System.Collections.Generic;
using System.IO;

namespace Shipwreck.CSharpModels.Expressions
{
    public sealed partial class ConditionalExpression : Expression
    {
        internal ConditionalExpression(Expression condition, Expression truePart, Expression falsePart)
        {
            condition.ArgumentIsNotNull(nameof(condition));
            truePart.ArgumentIsNotNull(nameof(truePart));
            falsePart.ArgumentIsNotNull(nameof(falsePart));

            Condition = condition;
            TruePart = truePart;
            FalsePart = falsePart;
        }

        public Expression Condition { get; }
        public Expression TruePart { get; }
        public Expression FalsePart { get; }

        public override Type Type
            => TruePart.Type.IsAssignableFrom(FalsePart.Type) ? TruePart.Type : FalsePart.Type;

        public override bool IsEqualTo(Syntax other)
            => this == (object)other
                || (other is ConditionalExpression be
                    && Condition.IsEqualTo(be.Condition)
                    && TruePart.IsEqualTo(be.TruePart)
                    && FalsePart.IsEqualTo(be.FalsePart));

        internal override Expression ReduceCore()
        {
            if (Condition.TryReduce(out var l) | TruePart.TryReduce(out var r) | FalsePart.TryReduce(out var f))
            {
                return new ConditionalExpression(l, r, f);
            }

            return this;
        }

        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Conditional;

        public override IEnumerable<Expression> GetChildren()
        {
            yield return Condition;
            yield return TruePart;
            yield return FalsePart;
        }
    }
}