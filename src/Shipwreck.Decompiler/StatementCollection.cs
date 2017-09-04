using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Shipwreck.Decompiler
{
    public sealed class StatementCollection : OwnedCollection<IStatementNode, Statement>
    {
        public StatementCollection()
        {
        }

        internal StatementCollection(IStatementNode owner)
            : base(owner)
        {
        }

        protected override void ClearState(Statement item)
        {
            item.Collection = null;
        }

        protected override void SetState(Statement item)
        {
            if (item.Collection != null)
            {
                throw new InvalidOperationException();
            }
            item.Collection = this;
        }
    }
}