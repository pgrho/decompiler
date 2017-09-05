using System;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class CatchClauseCollection : OwnedCollection<TryStatement, CatchClause>
    {
        public CatchClauseCollection()
        {
        }

        internal CatchClauseCollection(TryStatement owner)
            : base(owner)
        {
        }

        protected override void ClearState(CatchClause item)
        {
            item.TryStatement = null;
        }

        protected override void SetState(CatchClause item)
        {
            if (item.TryStatement != null && (item.TryStatement != Owner || item.TryStatement.CatchClauses.Contains(item)))
            {
                throw new InvalidOperationException();
            }
            item.TryStatement = Owner;
        }
    }
}