using System.Collections.Generic;
using System.Linq;

namespace Shipwreck.Decompiler
{
    public sealed class SyntaxContainer
    {
        private HashSet<SyntaxContainer> _From;
        private HashSet<SyntaxContainer> _To;

        public SyntaxContainer(int offset, Syntax syntax)
        {
            Offset = offset;
            Syntax = syntax;
        }

        public int Offset { get; }

        public Syntax Syntax { get; set; }

        public bool HasFrom
            => _From?.Count > 0;

        public IEnumerable<SyntaxContainer> From
            => _From?.AsEnumerable() ?? Enumerable.Empty<SyntaxContainer>();

        public bool HasTo
            => _To?.Count > 0;

        public IEnumerable<SyntaxContainer> To
            => _To?.AsEnumerable() ?? Enumerable.Empty<SyntaxContainer>();

        internal void SetTo(SyntaxContainer to)
        {
            if (_To != null)
            {
                foreach (var t in _To)
                {
                    t._From?.Remove(this);
                }
                _To.Clear();
            }
            if (to != null)
            {
                (_To ?? (_To = new HashSet<SyntaxContainer>())).Add(to);

                (to._From ?? (to._From = new HashSet<SyntaxContainer>())).Add(this);
            }
        }

        internal void SetTo(IEnumerable<SyntaxContainer> to)
        {
            if (_To != null)
            {
                foreach (var t in _To)
                {
                    t._From?.Remove(this);
                }
                _To.Clear();
            }
            if (to != null)
            {
                (_To ?? (_To = new HashSet<SyntaxContainer>())).UnionWith(to);

                foreach (var t in to)
                {
                    (t._From ?? (t._From = new HashSet<SyntaxContainer>())).Add(this);
                }
            }
        }

        internal void ClearTo()
            => SetTo((SyntaxContainer)null);
    }
}