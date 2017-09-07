using System;
using System.Collections.Generic;
using System.Linq;

namespace Shipwreck.Decompiler.Statements
{
    public sealed partial class LabelTarget : Statement
    {
        public LabelTarget(string name)
        {
            name.ArgumentIsNotNull(nameof(name));

            Name = name;
        }

        public string Name { get; }

        public override bool IsEqualTo(Syntax other)
            => this == (object)other
            || (other is LabelTarget lb && Name == lb.Name);

        public override bool Reduce()
        {
            if (Collection != null)
            {
                if (!ReferencedFrom().Any())
                {
                    Collection.Remove(this);
                    return true;
                }
            }
            return base.Reduce();
        }

        internal IEnumerable<GoToStatement> ReferencedFrom()
            => this.TreeStatements().OfType<GoToStatement>().Where(g => g.Target == this);

        public override Statement Clone()
            => throw new NotSupportedException();
    }
}