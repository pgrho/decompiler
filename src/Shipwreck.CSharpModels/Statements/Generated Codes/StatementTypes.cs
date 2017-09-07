namespace Shipwreck.Decompiler.Statements
{
    partial class BreakStatement
    {
        public override void AcceptVisitor(IStatementVisitor visitor)
            => visitor.VisitBreakStatement(this);

        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitBreakStatement(this);

        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitBreakStatement(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitBreakStatement(this, parameter);
    }

    partial class ConstantDeclarationStatement
    {
        public override void AcceptVisitor(IStatementVisitor visitor)
            => visitor.VisitConstantDeclarationStatement(this);

        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitConstantDeclarationStatement(this);

        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitConstantDeclarationStatement(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitConstantDeclarationStatement(this, parameter);
    }

    partial class ContinueStatement
    {
        public override void AcceptVisitor(IStatementVisitor visitor)
            => visitor.VisitContinueStatement(this);

        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitContinueStatement(this);

        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitContinueStatement(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitContinueStatement(this, parameter);
    }

    partial class DoWhileStatement
    {
        public override void AcceptVisitor(IStatementVisitor visitor)
            => visitor.VisitDoWhileStatement(this);

        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitDoWhileStatement(this);

        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitDoWhileStatement(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitDoWhileStatement(this, parameter);
    }

    partial class ExpressionStatement
    {
        public override void AcceptVisitor(IStatementVisitor visitor)
            => visitor.VisitExpressionStatement(this);

        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitExpressionStatement(this);

        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitExpressionStatement(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitExpressionStatement(this, parameter);
    }

    partial class ForEachStatement
    {
        public override void AcceptVisitor(IStatementVisitor visitor)
            => visitor.VisitForEachStatement(this);

        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitForEachStatement(this);

        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitForEachStatement(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitForEachStatement(this, parameter);
    }

    partial class ForStatement
    {
        public override void AcceptVisitor(IStatementVisitor visitor)
            => visitor.VisitForStatement(this);

        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitForStatement(this);

        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitForStatement(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitForStatement(this, parameter);
    }

    partial class GoToStatement
    {
        public override void AcceptVisitor(IStatementVisitor visitor)
            => visitor.VisitGoToStatement(this);

        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitGoToStatement(this);

        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitGoToStatement(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitGoToStatement(this, parameter);
    }

    partial class IfStatement
    {
        public override void AcceptVisitor(IStatementVisitor visitor)
            => visitor.VisitIfStatement(this);

        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitIfStatement(this);

        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitIfStatement(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitIfStatement(this, parameter);
    }

    partial class LabelTarget
    {
        public override void AcceptVisitor(IStatementVisitor visitor)
            => visitor.VisitLabelTarget(this);

        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitLabelTarget(this);

        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitLabelTarget(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitLabelTarget(this, parameter);
    }

    partial class LockStatement
    {
        public override void AcceptVisitor(IStatementVisitor visitor)
            => visitor.VisitLockStatement(this);

        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitLockStatement(this);

        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitLockStatement(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitLockStatement(this, parameter);
    }

    partial class ReturnStatement
    {
        public override void AcceptVisitor(IStatementVisitor visitor)
            => visitor.VisitReturnStatement(this);

        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitReturnStatement(this);

        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitReturnStatement(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitReturnStatement(this, parameter);
    }

    partial class SwitchStatement
    {
        public override void AcceptVisitor(IStatementVisitor visitor)
            => visitor.VisitSwitchStatement(this);

        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitSwitchStatement(this);

        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitSwitchStatement(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitSwitchStatement(this, parameter);
    }

    partial class ThrowStatement
    {
        public override void AcceptVisitor(IStatementVisitor visitor)
            => visitor.VisitThrowStatement(this);

        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitThrowStatement(this);

        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitThrowStatement(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitThrowStatement(this, parameter);
    }

    partial class TryStatement
    {
        public override void AcceptVisitor(IStatementVisitor visitor)
            => visitor.VisitTryStatement(this);

        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitTryStatement(this);

        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitTryStatement(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitTryStatement(this, parameter);
    }

    partial class UsingStatement
    {
        public override void AcceptVisitor(IStatementVisitor visitor)
            => visitor.VisitUsingStatement(this);

        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitUsingStatement(this);

        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitUsingStatement(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitUsingStatement(this, parameter);
    }

    partial class VariableDeclarationStatement
    {
        public override void AcceptVisitor(IStatementVisitor visitor)
            => visitor.VisitVariableDeclarationStatement(this);

        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitVariableDeclarationStatement(this);

        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitVariableDeclarationStatement(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitVariableDeclarationStatement(this, parameter);
    }

    partial class WhileStatement
    {
        public override void AcceptVisitor(IStatementVisitor visitor)
            => visitor.VisitWhileStatement(this);

        public override TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitWhileStatement(this);

        public override void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitWhileStatement(this, parameter);

        public override TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitWhileStatement(this, parameter);
    }

    partial interface IStatementVisitor
    {
        void VisitBreakStatement(BreakStatement breakStatement);
        void VisitConstantDeclarationStatement(ConstantDeclarationStatement constantDeclarationStatement);
        void VisitContinueStatement(ContinueStatement continueStatement);
        void VisitDoWhileStatement(DoWhileStatement doWhileStatement);
        void VisitExpressionStatement(ExpressionStatement expressionStatement);
        void VisitForEachStatement(ForEachStatement forEachStatement);
        void VisitForStatement(ForStatement forStatement);
        void VisitGoToStatement(GoToStatement goToStatement);
        void VisitIfStatement(IfStatement ifStatement);
        void VisitLabelTarget(LabelTarget labelTarget);
        void VisitLockStatement(LockStatement lockStatement);
        void VisitReturnStatement(ReturnStatement returnStatement);
        void VisitSwitchStatement(SwitchStatement switchStatement);
        void VisitThrowStatement(ThrowStatement throwStatement);
        void VisitTryStatement(TryStatement tryStatement);
        void VisitUsingStatement(UsingStatement usingStatement);
        void VisitVariableDeclarationStatement(VariableDeclarationStatement variableDeclarationStatement);
        void VisitWhileStatement(WhileStatement whileStatement);
    }

    partial interface IStatementVisitor<TResult>
    {
        TResult VisitBreakStatement(BreakStatement breakStatement);
        TResult VisitConstantDeclarationStatement(ConstantDeclarationStatement constantDeclarationStatement);
        TResult VisitContinueStatement(ContinueStatement continueStatement);
        TResult VisitDoWhileStatement(DoWhileStatement doWhileStatement);
        TResult VisitExpressionStatement(ExpressionStatement expressionStatement);
        TResult VisitForEachStatement(ForEachStatement forEachStatement);
        TResult VisitForStatement(ForStatement forStatement);
        TResult VisitGoToStatement(GoToStatement goToStatement);
        TResult VisitIfStatement(IfStatement ifStatement);
        TResult VisitLabelTarget(LabelTarget labelTarget);
        TResult VisitLockStatement(LockStatement lockStatement);
        TResult VisitReturnStatement(ReturnStatement returnStatement);
        TResult VisitSwitchStatement(SwitchStatement switchStatement);
        TResult VisitThrowStatement(ThrowStatement throwStatement);
        TResult VisitTryStatement(TryStatement tryStatement);
        TResult VisitUsingStatement(UsingStatement usingStatement);
        TResult VisitVariableDeclarationStatement(VariableDeclarationStatement variableDeclarationStatement);
        TResult VisitWhileStatement(WhileStatement whileStatement);
    }

    partial interface IParameteredStatementVisitor<TParameter>
    {
        void VisitBreakStatement(BreakStatement breakStatement, TParameter parameter);
        void VisitConstantDeclarationStatement(ConstantDeclarationStatement constantDeclarationStatement, TParameter parameter);
        void VisitContinueStatement(ContinueStatement continueStatement, TParameter parameter);
        void VisitDoWhileStatement(DoWhileStatement doWhileStatement, TParameter parameter);
        void VisitExpressionStatement(ExpressionStatement expressionStatement, TParameter parameter);
        void VisitForEachStatement(ForEachStatement forEachStatement, TParameter parameter);
        void VisitForStatement(ForStatement forStatement, TParameter parameter);
        void VisitGoToStatement(GoToStatement goToStatement, TParameter parameter);
        void VisitIfStatement(IfStatement ifStatement, TParameter parameter);
        void VisitLabelTarget(LabelTarget labelTarget, TParameter parameter);
        void VisitLockStatement(LockStatement lockStatement, TParameter parameter);
        void VisitReturnStatement(ReturnStatement returnStatement, TParameter parameter);
        void VisitSwitchStatement(SwitchStatement switchStatement, TParameter parameter);
        void VisitThrowStatement(ThrowStatement throwStatement, TParameter parameter);
        void VisitTryStatement(TryStatement tryStatement, TParameter parameter);
        void VisitUsingStatement(UsingStatement usingStatement, TParameter parameter);
        void VisitVariableDeclarationStatement(VariableDeclarationStatement variableDeclarationStatement, TParameter parameter);
        void VisitWhileStatement(WhileStatement whileStatement, TParameter parameter);
    }

    partial interface IParameteredStatementVisitor<TParameter, TResult>
    {
        TResult VisitBreakStatement(BreakStatement breakStatement, TParameter parameter);
        TResult VisitConstantDeclarationStatement(ConstantDeclarationStatement constantDeclarationStatement, TParameter parameter);
        TResult VisitContinueStatement(ContinueStatement continueStatement, TParameter parameter);
        TResult VisitDoWhileStatement(DoWhileStatement doWhileStatement, TParameter parameter);
        TResult VisitExpressionStatement(ExpressionStatement expressionStatement, TParameter parameter);
        TResult VisitForEachStatement(ForEachStatement forEachStatement, TParameter parameter);
        TResult VisitForStatement(ForStatement forStatement, TParameter parameter);
        TResult VisitGoToStatement(GoToStatement goToStatement, TParameter parameter);
        TResult VisitIfStatement(IfStatement ifStatement, TParameter parameter);
        TResult VisitLabelTarget(LabelTarget labelTarget, TParameter parameter);
        TResult VisitLockStatement(LockStatement lockStatement, TParameter parameter);
        TResult VisitReturnStatement(ReturnStatement returnStatement, TParameter parameter);
        TResult VisitSwitchStatement(SwitchStatement switchStatement, TParameter parameter);
        TResult VisitThrowStatement(ThrowStatement throwStatement, TParameter parameter);
        TResult VisitTryStatement(TryStatement tryStatement, TParameter parameter);
        TResult VisitUsingStatement(UsingStatement usingStatement, TParameter parameter);
        TResult VisitVariableDeclarationStatement(VariableDeclarationStatement variableDeclarationStatement, TParameter parameter);
        TResult VisitWhileStatement(WhileStatement whileStatement, TParameter parameter);
    }
}
