using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.IO;
using Shipwreck.CSharpModels.Expressions;

namespace Shipwreck.CSharpModels.Statements
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

        internal void WriteTo(IndentedTextWriter writer)
            => AcceptVisitor(CSharpSyntaxWriter.Default, writer);

        public override string ToString()
        {
            using (var sw = new StringWriter())
            using (var tw = new IndentedTextWriter(sw))
            {
                WriteTo(tw);

                tw.Flush();

                return sw.ToString();
            }
        }

        #region AcceptVisitor

        public void AcceptVisitor(IStatementVisitor visitor)
            => visitor.VisitSwitchSection(this);

        public TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitSwitchSection(this);

        public void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitSwitchSection(this, parameter);

        public TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitSwitchSection(this, parameter);

        #endregion AcceptVisitor
    }
}