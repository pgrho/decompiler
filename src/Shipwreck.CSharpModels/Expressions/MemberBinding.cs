using System.IO;
using System.Reflection;

namespace Shipwreck.CSharpModels.Expressions
{
    public abstract class MemberBinding
    {
        internal MemberBinding(MemberInfo member)
        {
            member.ArgumentIsNotNull(nameof(member));

            Member = member;
        }

        public MemberInfo Member { get; }

        public abstract bool IsEqualTo(MemberBinding other);
         
        internal abstract MemberBinding ReduceCore();

        internal abstract MemberBinding ReplaceCore(Expression currentExpression, Expression newExpression, bool replaceAll, bool allowConditional);

        #region AcceptVisitor

        public abstract void AcceptVisitor(IExpressionVisitor visitor);

        public abstract TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor);

        public abstract void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter);

        public abstract TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter);

        #endregion AcceptVisitor
    }
}