using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class MethodCallExpression : Expression
    {
        public MethodCallExpression(MethodInfo method, IEnumerable<Expression> parameters)
        {
            Method = method;
            Parameters = Array.AsReadOnly(parameters?.ToArray() ?? new Expression[0]);
        }

        public MethodCallExpression(Expression obj, MethodInfo method, IEnumerable<Expression> parameters)
        {
            Object = obj;
            Method = method;
            Parameters = Array.AsReadOnly(parameters?.ToArray() ?? new Expression[0]);
        }

        public Expression Object { get; }

        public MethodInfo Method { get; }

        public ReadOnlyCollection<Expression> Parameters { get; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is MethodCallExpression ne
                && (Object?.IsEquivalentTo(ne.Object) ?? ne.Object == null)
                && Method == ne.Method
                && Parameters.Count == ne.Parameters.Count
                && Enumerable.Range(0, Parameters.Count).All(i => Parameters[i].IsEquivalentTo(ne.Parameters[i])));

        public override void WriteTo(TextWriter writer)
        {
            if (Object != null)
            {
                writer.Write('(');
                Object.WriteTo(writer);
                writer.Write(')');
            }
            writer.Write(Method.Name);
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