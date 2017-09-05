using System.Collections.Generic;

namespace Shipwreck.Decompiler.Statements
{
    public interface IStatementNode
    {
        StatementCollection Collection { get; }

        IEnumerable<StatementCollection> GetChildCollections();
    }
}