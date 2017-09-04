using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class NewExpression : CallExpression
    {
        public NewExpression(Type type)
            : base(null)
        {
            Constructor = type.GetConstructor(Type.EmptyTypes);
        }

        public NewExpression(ConstructorInfo constructor, IEnumerable<Expression> parameters)
            : base(parameters)
        {
            Constructor = constructor;
        }
        internal NewExpression(ConstructorInfo constructor, IEnumerable<Expression> parameters, bool shouldCopy)
            : base(parameters, shouldCopy)
        {
            Constructor = constructor;
        }

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
    }
}