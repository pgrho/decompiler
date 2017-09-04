using System;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class CatchClause
    {
        public CatchClause(TryBlock block, Type type)
        {
            Block = block;
            CatchType = type;
        }

        public Type CatchType { get; }

        public TryBlock Block { get; internal set; }

        #region Statements

        private StatementCollection _Statements;

        public StatementCollection Statements
            => _Statements ?? (_Statements = new StatementCollection(Block));

        public bool ShouldSerializeStatements()
            => _Statements.ShouldSerialize();

        #endregion Statements
    }
}