using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed partial class MemberInitExpression : Expression
    {
        public MemberInitExpression(NewExpression newExpression, IEnumerable<MemberBinding> bindings)
        {
            newExpression.ArgumentIsNotNull(nameof(newExpression));
            bindings.ArgumentIsNotNull(nameof(bindings));

            NewExpression = newExpression;
            Bindings = Array.AsReadOnly(bindings.ToArray());
        }

        public NewExpression NewExpression { get; }

        public ReadOnlyCollection<MemberBinding> Bindings { get; }

        public override Type Type => NewExpression.Type;
        public override ExpressionPrecedence Precedence => ExpressionPrecedence.Primary;

        public override bool IsEquivalentTo(Syntax other)
        {
            if (this == other)
            {
                return true;
            }
            if (!(other is MemberInitExpression mie)
                || !NewExpression.IsEquivalentTo(mie.NewExpression)
                || Bindings.Count != mie.Bindings.Count)
            {
                return false;
            }
            for (int i = 0; i < Bindings.Count; i++)
            {
                if (!Bindings[i].IsEquivalentTo(mie.Bindings[i]))
                {
                    return false;
                }
            }
            return true;
        }

        internal override Expression ReduceCore()
        {
            var ne = NewExpression.Reduce();

            MemberBinding[] mbs = null;
            for (int i = 0; i < Bindings.Count; i++)
            {
                var b = Bindings[i];
                var rb = b.ReduceCore();
                if (rb != b)
                {
                    (mbs ?? (mbs = new MemberBinding[Bindings.Count]))[i] = rb;
                }
            }

            if (ne == NewExpression && mbs == null)
            {
                return this;
            }

            if (mbs == null)
            {
                mbs = new MemberBinding[Bindings.Count];
            }
            for (int i = 0; i < Bindings.Count; i++)
            {
                mbs[i] = mbs[i] ?? Bindings[i];
            }

            return new MemberInitExpression((NewExpression)ne, mbs);
        }

        internal override Expression ReplaceCore(Expression currentExpression, Expression newExpression, bool replaceAll, bool allowConditional)
        {
            var ne = NewExpression.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional);

            MemberBinding[] mbs = null;
            for (int i = 0; i < Bindings.Count; i++)
            {
                var b = Bindings[i];
                var rb = b.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional);
                if (rb != b)
                {
                    (mbs ?? (mbs = new MemberBinding[Bindings.Count]))[i] = rb;
                }
            }

            if (ne == NewExpression && mbs == null)
            {
                return this;
            }

            if (mbs == null)
            {
                mbs = new MemberBinding[Bindings.Count];
            }
            for (int i = 0; i < Bindings.Count; i++)
            {
                mbs[i] = mbs[i] ?? Bindings[i];
            }

            return new MemberInitExpression((NewExpression)ne, mbs);
        }

        public override IEnumerable<Expression> GetChildren()
        {
            yield return NewExpression;
            foreach (var mb in Bindings)
            {
                // TODO: consider assignment
                if (mb is MemberAssignment ma)
                {
                    yield return ma.Expression;
                }
            }
        }
    }
}