using System.IO;
using System.Reflection;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class MemberAssignment : MemberBinding
    {
        internal MemberAssignment(MemberInfo member, Expression expression)
            : base(member)
        {
            expression.ArgumentIsNotNull(nameof(expression));

            Expression = expression;
        }

        public Expression Expression { get; }

        public override bool IsEquivalentTo(MemberBinding other)
            => this == other
            || (other is MemberAssignment ma && ma.Member == Member && ma.Expression.IsEquivalentTo(Expression));

        public override void WriteTo(TextWriter writer)
        {
            writer.Write(Member.Name);
            writer.Write(" = ");
            Expression.WriteTo(writer);
        }

        internal override MemberBinding ReduceCore()
        {
            var e = Expression.Reduce();
            return e == Expression ? this : new MemberAssignment(Member, e);
        }

        internal override MemberBinding ReplaceCore(Expression currentExpression, Expression newExpression, bool replaceAll, bool allowConditional)
        {
            var e = Expression.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional);
            return e == Expression ? this : new MemberAssignment(Member, e);
        }
    }
}