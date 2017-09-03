using System.Collections.Generic;

namespace Shipwreck.Decompiler
{
    public interface IStatementNode
    {
        StatementCollection Collection { get; }

        IEnumerable<StatementCollection> GetChildCollections();
    }
}