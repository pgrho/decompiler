using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class NewExpression : Expression
    {
        public NewExpression(Type type)
        {
            Constructor = type.GetConstructor(Type.EmptyTypes);
            Parameters = new ReadOnlyCollection<Expression>(new Expression[0]);
        }

        public NewExpression(ConstructorInfo constructor, IEnumerable<Expression> parameters)
        {
            Constructor = constructor;
            Parameters = Array.AsReadOnly(parameters?.ToArray() ?? new Expression[0]);
        }

        public ConstructorInfo Constructor { get; }

        public ReadOnlyCollection<Expression> Parameters { get; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is NewExpression ne
                && Constructor == ne.Constructor
                && Parameters.Count == ne.Parameters.Count
                && Enumerable.Range(0, Parameters.Count).All(i => Parameters[i].IsEquivalentTo(ne.Parameters[i])));

        public override void WriteTo(TextWriter writer)
        {
            writer.Write("new ");
            writer.Write(Constructor.DeclaringType.FullName);
            writer.Write('(');
            for (int i = 0; i < Parameters.Count; i++)
            {
                if (i > 0)
                {
                    writer.Write(", ");
                }
                Parameters[i].WriteTo(writer);
            }
            writer.Write(')');
        }
    }
}