using System.Collections.Generic;
using System.Linq;

namespace Shipwreck.Decompiler.Statements
{
    public sealed partial class TryStatement : Statement
    {
        #region Block

        private StatementCollection _Block;

        public StatementCollection Block
            => _Block ?? (_Block = new StatementCollection(this));

        public bool ShouldSerializeBlock()
            => _Block.ShouldSerialize();

        #endregion Block

        #region CatchClauses

        private CatchClauseCollection _CatchClauses;

        public CatchClauseCollection CatchClauses
            => _CatchClauses ?? (_CatchClauses = new CatchClauseCollection(this));

        public bool ShouldSerializeCatchClauses()
            => _CatchClauses?.Count > 0;

        #endregion CatchClauses

        #region Finally

        private StatementCollection _Finally;

        public StatementCollection Finally
            => _Finally ?? (_Finally = new StatementCollection(this));

        public bool ShouldSerializeFinallyStatements()
            => _Finally.ShouldSerialize();

        #endregion Finally

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is TryStatement gt
                && _Block.IsEquivalentTo(gt._Block)
                && _Finally.IsEquivalentTo(gt._Finally));

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
                    var r = _Block.ReduceBlock()
                            | _Finally.ReduceBlock();
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
            if (ShouldSerializeBlock())
            {
                yield return _Block;
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
                yield return _Finally;
            }
        }

        public override Statement Clone()
        {
            var r = new TryStatement();
            if (ShouldSerializeBlock())
            {
                r.Block.AddRange(_Block.Select(s => s.Clone()));
            }

            if (ShouldSerializeCatchClauses())
            {
                foreach (var cc in CatchClauses)
                {
                    var nc = new CatchClause(r, cc.CatchType);

                    if (cc.ShouldSerializeStatements())
                    {
                        nc.Statements.AddRange(cc.Statements.Select(s => s.Clone()));
                    }

                    r.CatchClauses.Add(nc);
                }
            }

            if (ShouldSerializeFinallyStatements())
            {
                r.Finally.AddRange(_Finally.Select(s => s.Clone()));
            }
            return r;
        }
    }
}