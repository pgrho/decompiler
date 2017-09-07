using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Shipwreck.CSharpModels.Expressions
{
    public sealed partial class NewExpression : CallExpression
    {
        public NewExpression(Type type)
            : base(null)
        {
            Constructor = type.GetConstructor(Type.EmptyTypes);

            if (Constructor == null)
            {
                throw new ArgumentException();
            }
        }

        public NewExpression(ConstructorInfo constructor, IEnumerable<Expression> parameters)
            : base(parameters)
        {
            constructor.ArgumentIsNotNull(nameof(constructor));

            Constructor = constructor;
        }

        internal NewExpression(ConstructorInfo constructor, IEnumerable<Expression> parameters, bool shouldCopy)
            : base(parameters, shouldCopy)
        {
            constructor.ArgumentIsNotNull(nameof(constructor));

            Constructor = constructor;
        }

        public override Type Type
            => Constructor.DeclaringType;

        public ConstructorInfo Constructor { get; }

        public override bool IsEqualTo(Syntax other)
            => this == (object)other
            || (other is NewExpression ne
                && Constructor == ne.Constructor
                && base.IsEqualTo(other));

        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Primary;

        internal override Expression ReduceCore()
            => TryReduceParameters(out var ps) ? new NewExpression(Constructor, ps, false) : this;

        internal override Expression ReplaceCore(Expression currentExpression, Expression newExpression, bool replaceAll, bool allowConditional)
            => TryReplaceParameters(currentExpression, newExpression, replaceAll, allowConditional, out var ps)
                ? new NewExpression(Constructor, ps, false)
                : base.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional);

        public override IEnumerable<Expression> GetChildren()
        {
            foreach (var p in Parameters)
            {
                yield return p;
            }
        }
    }
}