using System;
using System.CodeDom.Compiler;
using System.IO;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class CatchClause
    {
        public CatchClause()
        {
        }

        public CatchClause(TryStatement tryStatement, Type type)
        {
            TryStatement = tryStatement;
            CatchType = type;
        }

        public Type CatchType { get; set; }

        private TryStatement _TryStatement;

        public TryStatement TryStatement
        {
            get => _TryStatement;
            internal set
            {
                if (value != _TryStatement)
                {
                    _TryStatement = value;
                    if (_Statements != null)
                    {
                        _Statements.Owner = value;
                    }
                }
            }
        }

        #region Statements

        private StatementCollection _Statements;

        public StatementCollection Statements
            => _Statements ?? (_Statements = new StatementCollection(TryStatement));

        public bool ShouldSerializeStatements()
            => _Statements.ShouldSerialize();

        #endregion Statements

        internal void WriteTo(IndentedTextWriter writer)
        {
            writer.Write("catch");

            if (CatchType != null && CatchType != typeof(object))
            {
                writer.Write(" (");
                writer.Write(CatchType.FullName);
                writer.Write(')');
            }

            writer.WriteLine();
            writer.WriteLine('{');
            if (ShouldSerializeStatements())
            {
                writer.Indent++;
                foreach (var s in Statements)
                {
                    s.WriteTo(writer);
                }
                writer.Indent--;
            }
            writer.WriteLine('}');
        }

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
    }
}