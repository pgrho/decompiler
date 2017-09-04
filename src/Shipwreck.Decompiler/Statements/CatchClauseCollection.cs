using System;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class CatchClauseCollection : OwnedCollection<TryBlock, CatchClause>
    {
        public CatchClauseCollection()
        {
        }

        internal CatchClauseCollection(TryBlock owner)
            : base(owner)
        {
        }

        protected override void ClearState(CatchClause item)
        {
            item.Block = null;
        }

        protected override void SetState(CatchClause item)
        {
            if (item.Block != null && (item.Block != Owner || item.Block.CatchClauses.Contains(item)))
            {
                throw new InvalidOperationException();
            }
            item.Block = Owner;
        }
    }
}