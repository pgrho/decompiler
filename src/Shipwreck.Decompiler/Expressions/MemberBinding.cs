using System.IO;
using System.Reflection;

namespace Shipwreck.Decompiler.Expressions
{
    public abstract class MemberBinding
    {
        internal MemberBinding(MemberInfo member)
        {
            member.ArgumentIsNotNull(nameof(member));

            Member = member;
        }

        public MemberInfo Member { get; }

        public abstract bool IsEquivalentTo(MemberBinding other);

        public abstract void WriteTo(TextWriter writer);

        internal abstract MemberBinding ReduceCore();

        internal abstract MemberBinding ReplaceCore(Expression currentExpression, Expression newExpression, bool replaceAll, bool allowConditional);
    }
}