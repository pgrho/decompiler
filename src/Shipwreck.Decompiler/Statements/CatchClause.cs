using System;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class CatchClause
    {
        public CatchClause()
        {
        }

        public CatchClause(TryStatement tryStatement, Type type)
        {
            TryStatement = tryStatement;
            CatchType = type;
        }

        public Type CatchType { get; set; }

        private TryStatement _TryStatement;

        public TryStatement TryStatement
        {
            get => _TryStatement;
            internal set
            {
                if (value != _TryStatement)
                {
                    _TryStatement = value;
                    if (_Statements != null)
                    {
                        _Statements.Owner = value;
                    }
                }
            }
        }

        #region Statements

        private StatementCollection _Statements;

        public StatementCollection Statements
            => _Statements ?? (_Statements = new StatementCollection(TryStatement));

        public bool ShouldSerializeStatements()
            => _Statements.ShouldSerialize();

        #endregion Statements
    }
}