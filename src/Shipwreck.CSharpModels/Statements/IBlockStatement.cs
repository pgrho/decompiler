namespace Shipwreck.CSharpModels.Statements
{
    /// <summary>
    /// Represents a statement with up to one block.
    /// </summary>
    internal interface IBlockStatement : IStatementNode
    {
        StatementCollection Statements { get; }

        bool ShouldSerializeStatements();
    }
}