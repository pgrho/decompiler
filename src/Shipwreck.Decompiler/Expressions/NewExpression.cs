using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class NewExpression : CallExpression
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

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is NewExpression ne
                && Constructor == ne.Constructor
                && base.IsEquivalentTo(other));

        public override void WriteTo(TextWriter writer)
        {
            writer.Write("new ");
            writer.Write(Constructor.DeclaringType.FullName);
            writer.Write('(');
            WriteParametersTo(writer);
            writer.Write(')');
        }

        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Primary;

        internal override Expression ReduceCore()
            => TryReduceParameters(out var ps) ? new NewExpression(Constructor, ps, false) : this;

        public override IEnumerable<Expression> GetChildren()
        {
            foreach (var p in Parameters)
            {
                yield return p;
            }
        }
    }
}