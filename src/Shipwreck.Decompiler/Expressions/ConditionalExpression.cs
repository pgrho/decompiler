using System;
using System.Collections.Generic;
using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class ConditionalExpression : Expression
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

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
                || (other is ConditionalExpression be
                    && Condition.IsEquivalentTo(be.Condition)
                    && TruePart.IsEquivalentTo(be.TruePart)
                    && FalsePart.IsEquivalentTo(be.FalsePart));

        public override void WriteTo(TextWriter writer)
        {
            writer.WriteSecondChild(Condition, this);
            writer.Write(" ? ");
            writer.WriteSecondChild(TruePart, this);
            writer.Write(" : ");
            writer.WriteFirstChild(FalsePart, this);
        }

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