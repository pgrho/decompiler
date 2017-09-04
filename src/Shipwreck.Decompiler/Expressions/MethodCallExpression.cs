using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class MethodCallExpression : CallExpression
    {
        public MethodCallExpression(MethodInfo method, IEnumerable<Expression> parameters)
            : base(parameters)
        {
            Method = method;
        }

        public MethodCallExpression(Expression obj, MethodInfo method, IEnumerable<Expression> parameters)
            : base(parameters)
        {
            Object = obj;
            Method = method;
        }

        internal MethodCallExpression(Expression obj, MethodInfo method, IEnumerable<Expression> parameters, bool shouldCopy)
            : base(parameters)
        {
            Object = obj;
            Method = method;
        }

        public Expression Object { get; }

        public MethodInfo Method { get; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is MethodCallExpression ne
                && (Object?.IsEquivalentTo(ne.Object) ?? ne.Object == null)
                && Method == ne.Method
                && base.IsEquivalentTo(other));

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
            WriteParametersTo(writer);
            writer.Write(')');
        }

        internal override Expression ReduceCore()
        {
            var bf = (Method.IsStatic ? BindingFlags.Static : BindingFlags.Instance)
                        | BindingFlags.Public | BindingFlags.NonPublic;

            if (Method.Name.StartsWith("get_")
                || Method.Name.StartsWith("set_"))
            {
                var p = Method.DeclaringType.GetProperty(Method.Name.Substring(4), bf);

                if (Method == p?.GetMethod || Method == p?.SetMethod)
                {
                    var ipc = p.GetIndexParameters().Length;
                    Expression e;
                    if (ipc == 0)
                    {
                        e = Object.Property(p);
                    }
                    else
                    {
                        e = Object.MakeIndex(Parameters.Take(ipc));
                    }

                    if (Method.Name[0] == 's')
                    {
                        e = e.Assign(Parameters.Last());
                    }
                    return e;
                }
            }
            else if (Method.Name.StartsWith("add_")
                        || Method.Name.StartsWith("remove_"))
            {
                var isAdd = Method.Name[0] == 'a';

                var e = Method.DeclaringType.GetEvent(Method.Name.Substring(isAdd ? 4 : 7), bf);

                if (Method == e?.AddMethod || Method == e?.RemoveMethod)
                {
                    var obj = Object.MakeMemberAccess(e);

                    return obj.Assign(Parameters[0], isAdd ? BinaryOperator.Add : BinaryOperator.Subtract);
                }
            }
            else if (Method.IsStatic && Method.IsPublic && Method.Name.StartsWith("op_"))
            {
            }

            return base.ReduceCore();
        }
    }
}