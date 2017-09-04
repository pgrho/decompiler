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

                    case "op_Addition":
                        return Parameters[0].Add(Parameters[1]);

                    case "op_Subtraction":
                        return Parameters[0].Subtract(Parameters[1]);

                    case "op_Multiply":
                        return Parameters[0].Multiply(Parameters[1]);

                    case "op_Division":
                        return Parameters[0].Divide(Parameters[1]);

                    case "op_Modulus":
                        return Parameters[0].Modulo(Parameters[1]);

                    case "op_Equality":
                        return Parameters[0].Equal(Parameters[1]);

                    case "op_Inequality":
                        return Parameters[0].NotEqual(Parameters[1]);

                    case "op_GreaterThan":
                        return Parameters[0].GreaterThan(Parameters[1]);

                    case "op_GreaterThanOrEqual":
                        return Parameters[0].GreaterThanOrEqual(Parameters[1]);

                    case "op_LessThan":
                        return Parameters[0].LessThan(Parameters[1]);

                    case "op_LessThanOrEqual":
                        return Parameters[0].LessThanOrEqual(Parameters[1]);

                    case "op_BitwiseAnd":
                        return Parameters[0].And(Parameters[1]);

                    case "op_BitwiseOr":
                        return Parameters[0].Or(Parameters[1]);

                    case "op_ExclusiveOr":
                        return Parameters[0].ExclusiveOr(Parameters[1]);

                    case "op_LeftShift":
                        return Parameters[0].LeftShift(Parameters[1]);

                    case "op_RightShift":
                        return Parameters[0].RightShift(Parameters[1]);

                    case "op_Implicit":
                    case "op_Explicit":
                        return Parameters[0].Convert(Method.ReturnType);
                }
            }

            return base.ReduceCore();
        }
    }
}