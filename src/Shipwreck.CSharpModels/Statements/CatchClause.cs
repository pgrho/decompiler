using System;
using System.CodeDom.Compiler;
using System.IO;

namespace Shipwreck.CSharpModels.Statements
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
            => visitor.VisitCatchClause(this);

        public TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor)
            => visitor.VisitCatchClause(this);

        public void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter)
            => visitor.VisitCatchClause(this, parameter);

        public TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter)
            => visitor.VisitCatchClause(this, parameter);

        #endregion AcceptVisitor
    }
}