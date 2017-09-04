using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Shipwreck.Decompiler
{
    public abstract class OwnedCollection<TOwner, TItem> : Collection<TItem>
        where TOwner : class
        where TItem : class
    {
        public OwnedCollection()
            : base(new List<TItem>())
        {
        }

        internal OwnedCollection(TOwner owner)
            : base(new List<TItem>())
        {
            Owner = owner;
        }

        public TOwner Owner { get; }

        protected List<TItem> ItemList
            => ((List<TItem>)Items);

        protected override void ClearItems()
        {
            if (Owner != null)
            {
                foreach (var c in this)
                {
                    ClearState(c);
                }
            }
            base.ClearItems();
        }

        protected override void InsertItem(int index, TItem item)
        {
            item.ArgumentIsNotNull(nameof(item));

            if (Owner != null)
            {
                SetState(item);
            }
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            if (Owner != null)
            {
                ClearState(this[index]);
            }
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, TItem item)
        {
            item.ArgumentIsNotNull(nameof(item));

            if (Owner != null)
            {
                ClearState(this[index]);
                SetState(item);
            }
            base.SetItem(index, item);
        }

        protected abstract void ClearState(TItem item);

        protected abstract void SetState(TItem item);

        public void AddRange(IEnumerable<TItem> values)
        {
            foreach (var v in values)
            {
                Add(v);
            }
        }

        public void InsertRange(int index, IEnumerable<TItem> values)
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
                    ClearState(ItemList[index + j]);
                }
            }
            ItemList.RemoveRange(index, count);
        }
    }
}