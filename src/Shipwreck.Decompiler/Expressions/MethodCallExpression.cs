using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class MethodCallExpression : CallExpression
    {
        public MethodCallExpression(MethodBase method, IEnumerable<Expression> parameters)
            : base(parameters)
        {
            Method = method;
        }

        public MethodCallExpression(MethodBase method, params Expression[] parameters)
            : base(parameters)
        {
            Method = method;
        }

        public MethodCallExpression(Expression obj, MethodBase method, IEnumerable<Expression> parameters)
            : base(parameters)
        {
            Object = obj;
            Method = method;
        }

        public MethodCallExpression(Expression obj, MethodBase method, params Expression[] parameters)
            : base(parameters)
        {
            Object = obj;
            Method = method;
        }

        internal MethodCallExpression(Expression obj, MethodBase method, IEnumerable<Expression> parameters, bool shouldCopy)
            : base(parameters)
        {
            Object = obj;
            Method = method;
        }

        public Expression Object { get; }

        public MethodBase Method { get; }

        public override Type Type
            => Method is MethodInfo mi ? mi.ReturnType : typeof(void);

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
                writer.WriteFirstChild(Object, this);
            }
            else
            {
                writer.Write(Method.DeclaringType.FullName);
            }
            writer.Write('.');
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
                        e = Object.MakeIndex(p, Parameters.Take(ipc));
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
                switch (Method.Name)
                {
                    // Ignored unary operators
                    case "op_True":
                    case "op_UnaryPlus":
                        return Parameters[0];

                    case "op_LogicalNot":
                    case "op_False":
                        return Parameters[0].LogicalNot();

                    case "op_UnaryNegation":
                        return Parameters[0].Negate();

                    case "op_OnesComplement":
                        return Parameters[0].OnesComplement();

                    case "op_Increment":
                        return Parameters[0].PreIncrement();

                    case "op_Decrement":
                        return Parameters[0].PreDecrement();

                    case "op_Implicit":
                    case "op_Explicit":
                        return Parameters[0].Convert(Type);

                    default:
                        if (BinaryOperatorHelper.FromMethodName(Method.Name, out var bop))
                        {
                            return Parameters[0].MakeBinary(Parameters[1], bop, (MethodInfo)Method);
                        }
                        break;
                }
            }

            {
                var obj = Object?.Reduce();

                if (obj != Object | TryReduceParameters(out var ps))
                {
                    return new MethodCallExpression(obj, Method, ps, false);
                }
            }
            return base.ReduceCore();
        }

        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Primary;
    }
}