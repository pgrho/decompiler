using System.Collections.Generic;
using Shipwreck.CSharpModels.Statements;

namespace Shipwreck.Decompiler
{
    public sealed class DecompiledMethod : IStatementNode
    {
        #region RootStatements

        private StatementCollection _RootStatements;

        public StatementCollection RootStatements
            => _RootStatements ?? (_RootStatements = new StatementCollection(this));

        #endregion RootStatements

        StatementCollection IStatementNode.Collection
            => null;

        public IEnumerable<StatementCollection> GetChildCollections()
        {
            if (_RootStatements?.Count > 0)
            {
                yield return _RootStatements;
            }
        }
    }
}