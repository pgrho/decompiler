using System;
using System.Collections.Generic;
using System.Linq;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed partial class ForEachStatement : Statement
    {
        public Type Type { get; set; }

        public string Identifier { get; set; }

        public Expression Expression { get; set; }

        #region Statements

        private StatementCollection _Statements;

        public StatementCollection Statements
            => _Statements ?? (_Statements = new StatementCollection(this));

        public bool ShouldSerializeStatements()
            => _Statements.ShouldSerialize();

        #endregion Statements

        public override bool IsEqualTo(Syntax other)
            => this == (object)other
            || (other is ForEachStatement ws
                && Type == ws.Type
                && Identifier == ws.Identifier
                && Expression.IsEqualTo(ws.Expression)
                && _Statements.IsEqualTo(ws._Statements));

        public override IEnumerable<StatementCollection> GetChildCollections()
        {
            if (ShouldSerializeStatements())
            {
                yield return _Statements;
            }
        }

        public override bool Reduce()
        {
            var thisReduced = Expression.TryReduce(out var nc);
            if (thisReduced)
            {
                Expression = nc;
            }

            if (Collection != null)
            {
                bool iterReduced;
                do
                {
                    iterReduced = _Statements.ReduceBlock();
                    thisReduced |= iterReduced;
                }
                while (iterReduced);

                // TODO: Determine the Statements is empty
            }

            return thisReduced;
        }

        public override Statement Clone()
        {
            var r = new ForEachStatement()
            {
                Type = Type,
                Identifier = Identifier,
                Expression = Expression
            };
            if (ShouldSerializeStatements())
            {
                r.Statements.AddRange(_Statements.Select(s => s.Clone()));
            }
            return r;
        }
    }
}