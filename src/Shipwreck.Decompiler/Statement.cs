using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shipwreck.Decompiler
{
    public abstract class Statement : Syntax, IStatementNode
    {
        public StatementCollection Collection { get; internal set; }

        public abstract void WriteTo(IndentedTextWriter writer);

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

        public virtual bool Reduce()
            => false;

        public virtual IEnumerable<StatementCollection> GetChildCollections()
            => Enumerable.Empty<StatementCollection>();

        public abstract Statement Clone();
    }
}