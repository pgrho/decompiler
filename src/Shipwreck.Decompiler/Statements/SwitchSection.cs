using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.IO;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class SwitchSection
    {
        #region SwitchStatement

        private SwitchStatement _SwitchStatement;

        public SwitchStatement SwitchStatement
        {
            get => _SwitchStatement;
            internal set
            {
                if (value != _SwitchStatement)
                {
                    _SwitchStatement = value;
                    if (_Statements != null)
                    {
                        _Statements.Owner = value;
                    }
                }
            }
        }

        #endregion SwitchStatement

        #region Labels

        private Collection<Expression> _Labels;

        public Collection<Expression> Labels
            => _Labels ?? (_Labels = new Collection<Expression>());

        public bool ShouldSerializeLabels()
            => _Labels?.Count > 0;

        #endregion Labels

        #region Statements

        private StatementCollection _Statements;

        public StatementCollection Statements
            => _Statements ?? (_Statements = new StatementCollection(SwitchStatement));

        public bool ShouldSerializeStatements()
            => _Statements.ShouldSerialize();

        #endregion Statements

        internal void WriteTo(IndentedTextWriter writer)
        {
            if (ShouldSerializeLabels() && ShouldSerializeStatements())
            {
                foreach (var v in Labels)
                {
                    if (v == null)
                    {
                        writer.WriteLine("default:");
                    }
                    else
                    {
                        writer.Write("case ");
                        v.WriteTo(writer);
                        writer.WriteLine(';');
                    }
                }

                writer.Indent++;
                foreach (var ss in Statements)
                {
                    ss.WriteTo(writer);
                }
                writer.Indent--;
            }
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