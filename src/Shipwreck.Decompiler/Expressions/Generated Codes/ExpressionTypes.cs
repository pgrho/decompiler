namespace Shipwreck.Decompiler.Expressions
{
    partial class AssignmentExpression
    {
        public override void AcceptVisitor(IExpressionVisitor visitor)
            => visitor.VisitAssignmentExpression(this);

        public override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor)
            => visitor.VisitAssignmentExpression(this);

        public override void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitAssignmentExpression(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitAssignmentExpression(this, parameter);
    }

    partial class AwaitExpression
    {
        public override void AcceptVisitor(IExpressionVisitor visitor)
            => visitor.VisitAwaitExpression(this);

        public override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor)
            => visitor.VisitAwaitExpression(this);

        public override void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitAwaitExpression(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitAwaitExpression(this, parameter);
    }

    partial class BaseExpression
    {
        public override void AcceptVisitor(IExpressionVisitor visitor)
            => visitor.VisitBaseExpression(this);

        public override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor)
            => visitor.VisitBaseExpression(this);

        public override void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitBaseExpression(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitBaseExpression(this, parameter);
    }

    partial class BinaryExpression
    {
        public override void AcceptVisitor(IExpressionVisitor visitor)
            => visitor.VisitBinaryExpression(this);

        public override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor)
            => visitor.VisitBinaryExpression(this);

        public override void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitBinaryExpression(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitBinaryExpression(this, parameter);
    }

    partial class ConditionalExpression
    {
        public override void AcceptVisitor(IExpressionVisitor visitor)
            => visitor.VisitConditionalExpression(this);

        public override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor)
            => visitor.VisitConditionalExpression(this);

        public override void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitConditionalExpression(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitConditionalExpression(this, parameter);
    }

    partial class ConstantExpression
    {
        public override void AcceptVisitor(IExpressionVisitor visitor)
            => visitor.VisitConstantExpression(this);

        public override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor)
            => visitor.VisitConstantExpression(this);

        public override void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitConstantExpression(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitConstantExpression(this, parameter);
    }

    partial class DefaultExpression
    {
        public override void AcceptVisitor(IExpressionVisitor visitor)
            => visitor.VisitDefaultExpression(this);

        public override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor)
            => visitor.VisitDefaultExpression(this);

        public override void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitDefaultExpression(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitDefaultExpression(this, parameter);
    }

    partial class IndexExpression
    {
        public override void AcceptVisitor(IExpressionVisitor visitor)
            => visitor.VisitIndexExpression(this);

        public override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor)
            => visitor.VisitIndexExpression(this);

        public override void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitIndexExpression(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitIndexExpression(this, parameter);
    }

    partial class MemberExpression
    {
        public override void AcceptVisitor(IExpressionVisitor visitor)
            => visitor.VisitMemberExpression(this);

        public override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor)
            => visitor.VisitMemberExpression(this);

        public override void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitMemberExpression(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitMemberExpression(this, parameter);
    }

    partial class MemberInitExpression
    {
        public override void AcceptVisitor(IExpressionVisitor visitor)
            => visitor.VisitMemberInitExpression(this);

        public override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor)
            => visitor.VisitMemberInitExpression(this);

        public override void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitMemberInitExpression(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitMemberInitExpression(this, parameter);
    }

    partial class MethodCallExpression
    {
        public override void AcceptVisitor(IExpressionVisitor visitor)
            => visitor.VisitMethodCallExpression(this);

        public override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor)
            => visitor.VisitMethodCallExpression(this);

        public override void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitMethodCallExpression(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitMethodCallExpression(this, parameter);
    }

    partial class NewArrayExpression
    {
        public override void AcceptVisitor(IExpressionVisitor visitor)
            => visitor.VisitNewArrayExpression(this);

        public override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor)
            => visitor.VisitNewArrayExpression(this);

        public override void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitNewArrayExpression(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitNewArrayExpression(this, parameter);
    }

    partial class NewExpression
    {
        public override void AcceptVisitor(IExpressionVisitor visitor)
            => visitor.VisitNewExpression(this);

        public override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor)
            => visitor.VisitNewExpression(this);

        public override void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitNewExpression(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitNewExpression(this, parameter);
    }

    partial class ParameterExpression
    {
        public override void AcceptVisitor(IExpressionVisitor visitor)
            => visitor.VisitParameterExpression(this);

        public override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor)
            => visitor.VisitParameterExpression(this);

        public override void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitParameterExpression(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitParameterExpression(this, parameter);
    }

    partial class ThisExpression
    {
        public override void AcceptVisitor(IExpressionVisitor visitor)
            => visitor.VisitThisExpression(this);

        public override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor)
            => visitor.VisitThisExpression(this);

        public override void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitThisExpression(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitThisExpression(this, parameter);
    }

    partial class TypeBinaryExpression
    {
        public override void AcceptVisitor(IExpressionVisitor visitor)
            => visitor.VisitTypeBinaryExpression(this);

        public override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor)
            => visitor.VisitTypeBinaryExpression(this);

        public override void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitTypeBinaryExpression(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitTypeBinaryExpression(this, parameter);
    }

    partial class UnaryExpression
    {
        public override void AcceptVisitor(IExpressionVisitor visitor)
            => visitor.VisitUnaryExpression(this);

        public override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor)
            => visitor.VisitUnaryExpression(this);

        public override void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitUnaryExpression(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitUnaryExpression(this, parameter);
    }

    partial class VariableExpression
    {
        public override void AcceptVisitor(IExpressionVisitor visitor)
            => visitor.VisitVariableExpression(this);

        public override TResult AcceptVisitor<TResult>(IExpressionVisitor<TResult> visitor)
            => visitor.VisitVariableExpression(this);

        public override void AcceptVisitor<TParameter>(IParameteredExpressionVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitVariableExpression(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredExpressionVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitVariableExpression(this, parameter);
    }

    partial interface IExpressionVisitor
    {
        void VisitAssignmentExpression(AssignmentExpression assignmentExpression);
        void VisitAwaitExpression(AwaitExpression awaitExpression);
        void VisitBaseExpression(BaseExpression baseExpression);
        void VisitBinaryExpression(BinaryExpression binaryExpression);
        void VisitConditionalExpression(ConditionalExpression conditionalExpression);
        void VisitConstantExpression(ConstantExpression constantExpression);
        void VisitDefaultExpression(DefaultExpression defaultExpression);
        void VisitIndexExpression(IndexExpression indexExpression);
        void VisitMemberExpression(MemberExpression memberExpression);
        void VisitMemberInitExpression(MemberInitExpression memberInitExpression);
        void VisitMethodCallExpression(MethodCallExpression methodCallExpression);
        void VisitNewArrayExpression(NewArrayExpression newArrayExpression);
        void VisitNewExpression(NewExpression newExpression);
        void VisitParameterExpression(ParameterExpression parameterExpression);
        void VisitThisExpression(ThisExpression thisExpression);
        void VisitTypeBinaryExpression(TypeBinaryExpression typeBinaryExpression);
        void VisitUnaryExpression(UnaryExpression unaryExpression);
        void VisitVariableExpression(VariableExpression variableExpression);
    }

    partial interface IExpressionVisitor<TResult>
    {
        TResult VisitAssignmentExpression(AssignmentExpression assignmentExpression);
        TResult VisitAwaitExpression(AwaitExpression awaitExpression);
        TResult VisitBaseExpression(BaseExpression baseExpression);
        TResult VisitBinaryExpression(BinaryExpression binaryExpression);
        TResult VisitConditionalExpression(ConditionalExpression conditionalExpression);
        TResult VisitConstantExpression(ConstantExpression constantExpression);
        TResult VisitDefaultExpression(DefaultExpression defaultExpression);
        TResult VisitIndexExpression(IndexExpression indexExpression);
        TResult VisitMemberExpression(MemberExpression memberExpression);
        TResult VisitMemberInitExpression(MemberInitExpression memberInitExpression);
        TResult VisitMethodCallExpression(MethodCallExpression methodCallExpression);
        TResult VisitNewArrayExpression(NewArrayExpression newArrayExpression);
        TResult VisitNewExpression(NewExpression newExpression);
        TResult VisitParameterExpression(ParameterExpression parameterExpression);
        TResult VisitThisExpression(ThisExpression thisExpression);
        TResult VisitTypeBinaryExpression(TypeBinaryExpression typeBinaryExpression);
        TResult VisitUnaryExpression(UnaryExpression unaryExpression);
        TResult VisitVariableExpression(VariableExpression variableExpression);
    }

    partial interface IParameteredExpressionVisitor<TParameter>
    {
        void VisitAssignmentExpression(AssignmentExpression assignmentExpression, TParameter parameter);
        void VisitAwaitExpression(AwaitExpression awaitExpression, TParameter parameter);
        void VisitBaseExpression(BaseExpression baseExpression, TParameter parameter);
        void VisitBinaryExpression(BinaryExpression binaryExpression, TParameter parameter);
        void VisitConditionalExpression(ConditionalExpression conditionalExpression, TParameter parameter);
        void VisitConstantExpression(ConstantExpression constantExpression, TParameter parameter);
        void VisitDefaultExpression(DefaultExpression defaultExpression, TParameter parameter);
        void VisitIndexExpression(IndexExpression indexExpression, TParameter parameter);
        void VisitMemberExpression(MemberExpression memberExpression, TParameter parameter);
        void VisitMemberInitExpression(MemberInitExpression memberInitExpression, TParameter parameter);
        void VisitMethodCallExpression(MethodCallExpression methodCallExpression, TParameter parameter);
        void VisitNewArrayExpression(NewArrayExpression newArrayExpression, TParameter parameter);
        void VisitNewExpression(NewExpression newExpression, TParameter parameter);
        void VisitParameterExpression(ParameterExpression parameterExpression, TParameter parameter);
        void VisitThisExpression(ThisExpression thisExpression, TParameter parameter);
        void VisitTypeBinaryExpression(TypeBinaryExpression typeBinaryExpression, TParameter parameter);
        void VisitUnaryExpression(UnaryExpression unaryExpression, TParameter parameter);
        void VisitVariableExpression(VariableExpression variableExpression, TParameter parameter);
    }

    partial interface IParameteredExpressionVisitor<TParameter, TResult>
    {
        TResult VisitAssignmentExpression(AssignmentExpression assignmentExpression, TParameter parameter);
        TResult VisitAwaitExpression(AwaitExpression awaitExpression, TParameter parameter);
        TResult VisitBaseExpression(BaseExpression baseExpression, TParameter parameter);
        TResult VisitBinaryExpression(BinaryExpression binaryExpression, TParameter parameter);
        TResult VisitConditionalExpression(ConditionalExpression conditionalExpression, TParameter parameter);
        TResult VisitConstantExpression(ConstantExpression constantExpression, TParameter parameter);
        TResult VisitDefaultExpression(DefaultExpression defaultExpression, TParameter parameter);
        TResult VisitIndexExpression(IndexExpression indexExpression, TParameter parameter);
        TResult VisitMemberExpression(MemberExpression memberExpression, TParameter parameter);
        TResult VisitMemberInitExpression(MemberInitExpression memberInitExpression, TParameter parameter);
        TResult VisitMethodCallExpression(MethodCallExpression methodCallExpression, TParameter parameter);
        TResult VisitNewArrayExpression(NewArrayExpression newArrayExpression, TParameter parameter);
        TResult VisitNewExpression(NewExpression newExpression, TParameter parameter);
        TResult VisitParameterExpression(ParameterExpression parameterExpression, TParameter parameter);
        TResult VisitThisExpression(ThisExpression thisExpression, TParameter parameter);
        TResult VisitTypeBinaryExpression(TypeBinaryExpression typeBinaryExpression, TParameter parameter);
        TResult VisitUnaryExpression(UnaryExpression unaryExpression, TParameter parameter);
        TResult VisitVariableExpression(VariableExpression variableExpression, TParameter parameter);
    }
}
