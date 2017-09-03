using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Shipwreck.Decompiler
{
    public sealed class StatementCollection : Collection<Statement>
    {
        public StatementCollection()
            : base(new List<Statement>())
        {
        }

        internal StatementCollection(IStatementNode owner)
            : base(new List<Statement>())
        {
            Owner = owner;
        }

        public IStatementNode Owner { get; }

        private List<Statement> ItemList
            => ((List<Statement>)Items);

        protected override void ClearItems()
        {
            if (Owner != null)
            {
                foreach (var c in this)
                {
                    c.Collection = null;
                }
            }
            base.ClearItems();
        }

        protected override void InsertItem(int index, Statement item)
        {
            item.ArgumentIsNotNull(nameof(item));

            if (Owner != null)
            {
                if (item.Collection != null)
                {
                    throw new InvalidOperationException();
                }
                item.Collection = this;
            }
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            if (Owner != null)
            {
                this[index].Collection = null;
            }
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, Statement item)
        {
            item.ArgumentIsNotNull(nameof(item));

            if (Owner != null)
            {
                this[index].Collection = null;
                if (item.Collection != null)
                {
                    throw new InvalidOperationException();
                }
                item.Collection = this;
            }
            base.SetItem(index, item);
        }

        public void AddRange(IEnumerable<Statement> values)
        {
            foreach (var v in values)
            {
                Add(v);
            }
        }

        public void InsertRange(int index, IEnumerable<Statement> values)
        {
            foreach (var v in values)
            {
                Insert(index++, v);
            }
        }

        public void RemoveRange(int index, int count)
        {
            if (Owner != null)
            {
                for (var j = 0; j < count; j++)
                {
                    ItemList[index + j].Collection = null;
                }
            }
            ItemList.RemoveRange(index, count);
        }
    }
}