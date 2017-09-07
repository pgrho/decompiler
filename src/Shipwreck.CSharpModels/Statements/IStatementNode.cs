using System.Collections.Generic;

namespace Shipwreck.CSharpModels.Statements
{
    public interface IStatementNode
    {
        StatementCollection Collection { get; }

        IEnumerable<StatementCollection> GetChildCollections();
    }
}