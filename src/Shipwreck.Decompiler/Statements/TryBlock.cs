using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class TryBlock : Statement
    {
        #region TryStatements

        private StatementCollection _TryStatements;

        public StatementCollection TryStatements
            => _TryStatements ?? (_TryStatements = new StatementCollection(this));

        public bool ShouldSerializeTryStatements()
            => _TryStatements.ShouldSerialize();

        #endregion TryStatements

        #region CatchClauses

        private CatchClauseCollection _CatchClauses;

        public CatchClauseCollection CatchClauses
            => _CatchClauses ?? (_CatchClauses = new CatchClauseCollection(this));

        public bool ShouldSerializeCatchClauses()
            => _CatchClauses?.Count > 0;

        #endregion CatchClauses

        #region FinallyStatements

        private StatementCollection _FinallyStatements;

        public StatementCollection FinallyStatements
            => _FinallyStatements ?? (_FinallyStatements = new StatementCollection(this));

        public bool ShouldSerializeFinallyStatements()
            => _FinallyStatements.ShouldSerialize();

        #endregion FinallyStatements

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is TryBlock gt
                && _TryStatements.IsEquivalentTo(gt._TryStatements)
                && _FinallyStatements.IsEquivalentTo(gt._FinallyStatements));

        public override void WriteTo(IndentedTextWriter writer)
        {
            writer.WriteLine("try");
            writer.WriteLine('{');
            if (ShouldSerializeTryStatements())
            {
                writer.Indent++;
                foreach (var s in _TryStatements)
                {
                    s.WriteTo(writer);
                }
                writer.Indent--;
            }
            writer.WriteLine('}');

            if (ShouldSerializeCatchClauses())
            {
                foreach (var c in _CatchClauses)
                {
                    writer.Write("catch");

                    if (c.CatchType != null && c.CatchType != typeof(object))
                    {
                        writer.Write(" (");
                        writer.Write(c.CatchType.FullName);
                        writer.Write(')');
                    }

                    writer.WriteLine();
                    writer.WriteLine('{');
                    if (c.ShouldSerializeStatements())
                    {
                        writer.Indent++;
                        foreach (var s in c.Statements)
                        {
                            s.WriteTo(writer);
                        }
                        writer.Indent--;
                    }
                    writer.WriteLine('}');
                }
            }

            if (ShouldSerializeFinallyStatements())
            {
                writer.WriteLine("finally");
                writer.WriteLine('{');
                writer.Indent++;
                foreach (var s in _FinallyStatements)
                {
                    s.WriteTo(writer);
                }
                writer.Indent--;
                writer.WriteLine('}');
            }
        }

        public override bool Reduce()
        {
            var thisReduced = false;

            if (Collection != null)
            {
                bool iterReduced;
                do
                {
                    if (Collection == null)
                    {
                        return true;
                    }

                    iterReduced = false;
                    var r = _TryStatements.ReduceBlock()
                            | _FinallyStatements.ReduceBlock();
                    thisReduced |= r;
                    iterReduced |= r;

                    if (ShouldSerializeCatchClauses())
                    {
                        foreach (var cc in _CatchClauses)
                        {
                            if (cc.ShouldSerializeStatements())
                            {
                                var r2 = cc.Statements.ReduceBlock();
                                thisReduced |= r2;
                                iterReduced |= r2;
                            }
                        }
                    }
                } while (iterReduced);
            }

            return thisReduced;
        }

        public override IEnumerable<StatementCollection> GetChildCollections()
        {
            if (ShouldSerializeTryStatements())
            {
                yield return _TryStatements;
            }

            if (ShouldSerializeCatchClauses())
            {
                foreach (var c in CatchClauses)
                {
                    if (c.ShouldSerializeStatements())
                    {
                        yield return c.Statements;
                    }
                }
            }

            if (ShouldSerializeFinallyStatements())
            {
                yield return _FinallyStatements;
            }
        }
    }
}