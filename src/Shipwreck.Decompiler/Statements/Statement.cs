using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shipwreck.Decompiler.Statements
{
    public abstract class Statement : Syntax, IStatementNode
    {
        public StatementCollection Collection { get; internal set; }

        public void WriteTo(IndentedTextWriter writer)
            => AcceptVisitor(CSharpSyntaxWriter.Default, writer);

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

        #region AcceptVisitor

        public abstract void AcceptVisitor(IStatementVisitor visitor);

        public abstract TResult AcceptVisitor<TResult>(IStatementVisitor<TResult> visitor);

        public abstract void AcceptVisitor<TParameter>(IParameteredStatementVisitor<TParameter> visitor, TParameter parameter);

        public abstract TResult AcceptVisitor<TParameter, TResult>(IParameteredStatementVisitor<TParameter, TResult> visitor, TParameter parameter);

        #endregion AcceptVisitor
    }
}