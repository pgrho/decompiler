using System.Collections.ObjectModel;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class SwitchSection
    {
        #region SwitchStatement

        private SwitchStatement _SwitchStatement;

        public SwitchStatement SwitchStatement
        {
            get => _SwitchStatement;
            internal set
            {
                if (value != _SwitchStatement)
                {
                    _SwitchStatement = value;
                    if (_Statements != null)
                    {
                        _Statements.Owner = value;
                    }
                }
            }
        }

        #endregion SwitchStatement

        #region Labels

        private Collection<Expression> _Labels;

        public Collection<Expression> Labels
            => _Labels ?? (_Labels = new Collection<Expression>());

        public bool ShouldSerializeLabels()
            => _Labels?.Count > 0;

        #endregion Labels

        #region Statements

        private StatementCollection _Statements;

        public StatementCollection Statements
            => _Statements ?? (_Statements = new StatementCollection(SwitchStatement));

        public bool ShouldSerializeStatements()
            => _Statements.ShouldSerialize();

        #endregion Statements
    }
}