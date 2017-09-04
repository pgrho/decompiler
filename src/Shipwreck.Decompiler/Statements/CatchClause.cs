using System;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class CatchClause
    {
        public CatchClause(TryStatement block, Type type)
        {
            Block = block;
            CatchType = type;
        }

        public Type CatchType { get; }

        public TryStatement Block { get; internal set; }

        #region Statements

        private StatementCollection _Statements;

        public StatementCollection Statements
            => _Statements ?? (_Statements = new StatementCollection(Block));

        public bool ShouldSerializeStatements()
            => _Statements.ShouldSerialize();

        #endregion Statements
    }
}