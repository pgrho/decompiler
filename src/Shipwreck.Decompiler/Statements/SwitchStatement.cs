using System.CodeDom.Compiler;
using System.Collections.Generic;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class SwitchStatement : Statement
    {
        public SwitchStatement()
        {
            Expression = 0.ToExpression();
        }

        public SwitchStatement(Expression expression)
        {
            Expression = expression ?? 0.ToExpression();
        }

        public Expression Expression { get; set; }

        #region Sections

        private SwitchSectionCollection _Sections;

        public SwitchSectionCollection Sections
            => _Sections ?? (_Sections = new SwitchSectionCollection(this));

        public bool ShouldSerializeSections()
            => _Sections?.Count > 0;

        #endregion Sections

        public override void WriteTo(IndentedTextWriter writer)
        {
            writer.Write("switch (");
            Expression.WriteTo(writer);
            writer.WriteLine(')');
            writer.WriteLine('{');
            if (ShouldSerializeSections())
            {
                writer.Indent++;
                foreach (var s in _Sections)
                {
                    if (s.ShouldSerializeLabels() && s.ShouldSerializeStatements())
                    {
                        foreach (var v in s.Labels)
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
                        foreach (var ss in s.Statements)
                        {
                            ss.WriteTo(writer);
                        }
                        writer.Indent--;
                    }
                }
                writer.Indent--;
            }
            writer.WriteLine('}');
        }

        public override Statement Clone()
        {
            var r = new SwitchStatement(Expression);

            if (ShouldSerializeSections())
            {
                foreach (var s in Sections)
                {
                    var ns = new SwitchSection();
                    foreach (var v in s.Labels)
                    {
                        ns.Labels.Add(v);
                    }
                    foreach (var v in s.Statements)
                    {
                        ns.Statements.Add(v.Clone());
                    }
                    r.Sections.Add(ns);
                }
            }
            return r;
        }

        public override bool IsEquivalentTo(Syntax other)
        {
            if (other == this)
            {
                return true;
            }
            if (other is SwitchStatement ss
                && Expression.IsEquivalentTo(ss.Expression)
                && (_Sections?.Count ?? 0) == (ss._Sections?.Count ?? 0))
            {
                if (ShouldSerializeSections())
                {
                    for (int i = 0; i < Sections.Count; i++)
                    {
                        var ms = Sections[i];
                        var os = ss.Sections[i];

                        if (ms.Labels.Count != os.Labels.Count)
                        {
                            return false;
                        }
                        for (int j = 0; j < ms.Labels.Count; j++)
                        {
                            var mv = ms.Labels[j];
                            var ov = os.Labels[j];

                            if (!(mv?.IsEquivalentTo(ov) ?? ov == null))
                            {
                                return false;
                            }
                        }

                        if (!ms.Statements.IsEquivalentTo(os.Statements))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            return false;
        }

        public override IEnumerable<StatementCollection> GetChildCollections()
        {
            if (ShouldSerializeSections())
            {
                foreach (var s in Sections)
                {
                    if (s.ShouldSerializeStatements())
                    {
                        yield return s.Statements;
                    }
                }
            }
        }
    }
}