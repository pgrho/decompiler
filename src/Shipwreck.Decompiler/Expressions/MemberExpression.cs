using System;
using System.IO;
using System.Reflection;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class MemberExpression : Expression
    {
        public MemberExpression(MemberInfo member)
            : this(null, member)
        {
        }

        public MemberExpression(Expression @object, MemberInfo member)
        {
            member.ArgumentIsNotNull(nameof(member));

            bool isStatic;
            if (member is PropertyInfo p)
            {
                isStatic = (p.GetMethod ?? p.SetMethod).IsStatic;
            }
            else if (member is FieldInfo f)
            {
                isStatic = f.IsStatic;
            }
            else if (member is EventInfo e)
            {
                isStatic = (e.AddMethod ?? e.RemoveMethod).IsStatic;
            }
            else
            {
                throw new ArgumentException($"{nameof(member)} must be a {nameof(PropertyInfo)}, {nameof(FieldInfo)} or {nameof(EventInfo)}");
            }

            if (@object == null)
            {
                if (!isStatic)
                {
                    throw new ArgumentException($"{nameof(@object)} must be specified if the {nameof(member)} is not static.");
                }
            }
            else
            {
                // TODO: if((isStatic || property.DeclaringType.IsAssignableFrom(@object.Type)){ throw; }
            }

            Object = @object;
            Member = member;
        }

        public Expression Object { get; }

        public MemberInfo Member { get; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is MemberExpression ne
                && (Object?.IsEquivalentTo(ne.Object) ?? ne.Object == null)
                && Member == ne.Member);

        public override void WriteTo(TextWriter writer)
        {
            if (Object == null)
            {
                writer.Write(Member.DeclaringType.FullName);
            }
            else
            {
                writer.Write('(');
                Object.WriteTo(writer);
                writer.Write(')');
            }
            writer.Write('.');
            writer.Write(Member.Name);
        }

        internal override Expression ReduceCore()
        {
            var o = Object?.ReduceCore();
            return o != Object ? o?.MakeMemberAccess(Member) : this;
        }

        internal override Expression ReplaceCore(Expression currentExpression, Expression newExpression, bool replaceAll, bool allowConditional)
        {
            var o = Object?.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional);
            return o != Object ? o?.MakeMemberAccess(Member) : this;
        }
    }
}