using System.Collections.Generic;
using System.Linq;

namespace Shipwreck.CSharpModels.Statements
{
    public sealed partial class UsingStatement : Statement, IBlockStatement
    {
        public UsingStatement()
        {
        }

        public UsingStatement(Statement resource)
        {
            Resource = resource;
        }

        public Statement Resource { get; set; }

        #region Statements

        private StatementCollection _Statements;

        public StatementCollection Statements
            => _Statements ?? (_Statements = new StatementCollection(this));

        public bool ShouldSerializeStatements()
            => _Statements.ShouldSerialize();

        #endregion Statements

        public override bool IsEqualTo(Syntax other)
            => this == (object)other
            || (other is UsingStatement ws
                && Resource.IsEqualTo(ws.Resource)
                && _Statements.IsEqualTo(ws._Statements));

        public override IEnumerable<StatementCollection> GetChildCollections()
        {
            if (ShouldSerializeStatements())
            {
                yield return _Statements;
            }
        }

        public override bool Reduce()
        {
            var thisReduced = Resource?.Reduce() ?? false;

            if (Collection != null)
            {
                bool iterReduced;
                do
                {
                    iterReduced = _Statements.ReduceBlock();
                    thisReduced |= iterReduced;
                }
                while (iterReduced);

                // TODO: Determine the Statements is empty
            }

            return thisReduced;
        }

        public override Statement Clone()
        {
            var r = new UsingStatement(Resource);
            if (ShouldSerializeStatements())
            {
                r.Statements.AddRange(_Statements.Select(s => s.Clone()));
            }
            return r;
        }
    }
}