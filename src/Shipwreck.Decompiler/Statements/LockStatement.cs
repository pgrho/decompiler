using System.Collections.Generic;
using System.Linq;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed partial class LockStatement : Statement
    {
        public LockStatement()
        {
        }

        public LockStatement(Expression @object)
        {
            Object = @object;
        }

        public Expression Object { get; set; }

        #region Statements

        private StatementCollection _Statements;

        public StatementCollection Statements
            => _Statements ?? (_Statements = new StatementCollection(this));

        public bool ShouldSerializeStatements()
            => _Statements.ShouldSerialize();

        #endregion Statements

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is LockStatement ws
                && Object.IsEquivalentTo(ws.Object)
                && _Statements.IsEquivalentTo(ws._Statements));

        public override IEnumerable<StatementCollection> GetChildCollections()
        {
            if (ShouldSerializeStatements())
            {
                yield return _Statements;
            }
        }

        public override bool Reduce()
        {
            var thisReduced = Object.TryReduce(out var nc);
            if (thisReduced)
            {
                Object = nc;
            }

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
            var r = new LockStatement(Object);
            if (ShouldSerializeStatements())
            {
                r.Statements.AddRange(_Statements.Select(s => s.Clone()));
            }
            return r;
        }
    }
}