namespace Shipwreck.Decompiler.Statements
{
    internal interface IContinuableStatement : IStatementNode
    {
        StatementCollection Statements { get; }

        bool ShouldSerializeStatements();
    }
}