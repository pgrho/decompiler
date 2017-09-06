namespace Shipwreck.Decompiler.Statements
{
    public partial interface IStatementVisitor
    {
        void VisitCatchClause(CatchClause catchClause);

        void VisitSwitchSection(SwitchSection switchSection);
    }

    public partial interface IStatementVisitor<TResult>
    {
        TResult VisitCatchClause(CatchClause catchClause);

        TResult VisitSwitchSection(SwitchSection switchSection);
    }

    public partial interface IParameteredStatementVisitor<TParameter>
    {
        void VisitCatchClause(CatchClause catchClause, TParameter parameter);

        void VisitSwitchSection(SwitchSection switchSection, TParameter parameter);
    }

    public partial interface IParameteredStatementVisitor<TParameter, TResult>
    {
        TResult VisitCatchClause(CatchClause catchClause, TParameter parameter);

        TResult VisitSwitchSection(SwitchSection switchSection, TParameter parameter);
    }
}