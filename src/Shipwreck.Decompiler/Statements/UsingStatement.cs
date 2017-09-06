using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class UsingStatement : Statement
    {
        public UsingStatement()
        {
        }

        public UsingStatement(Statement resource)
        {
            Resource = resource;
        }

        public Statement Resource { get; set; }

        #region Statements

        private StatementCollection _Statements;

        public StatementCollection Statements
            => _Statements ?? (_Statements = new StatementCollection(this));

        public bool ShouldSerializeStatements()
            => _Statements.ShouldSerialize();

        #endregion Statements

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is UsingStatement ws
                && Resource.IsEquivalentTo(ws.Resource)
                && _Statements.IsEquivalentTo(ws._Statements));

        public override void WriteTo(IndentedTextWriter writer)
        {
            writer.Write("using (");

            if (Resource is VariableDeclarationStatement ds)
            {
                if (ds.Declarators.Count == 1)
                {
                    ds.WriteDeclaration(writer);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else if (Resource is ExpressionStatement es)
            {
                es.Expression.WriteTo(writer);
            }
            else
            {
                throw new InvalidOperationException();
            }

            Resource.WriteTo(writer);
            writer.WriteLine(")");

            if (_Statements?.Count == 1 && _Statements[0] is UsingStatement)
            {
                _Statements[0].WriteTo(writer);
            }
            else
            {
                writer.WriteLine('{');
                if (ShouldSerializeStatements())
                {
                    writer.Indent++;
                    foreach (var s in _Statements)
                    {
                        s.WriteTo(writer);
                    }
                    writer.Indent--;
                }
                writer.WriteLine('}');
            }
        }

        public override IEnumerable<StatementCollection> GetChildCollections()
        {
            if (ShouldSerializeStatements())
            {
                yield return _Statements;
            }
        }

        public override bool Reduce()
        {
            var thisReduced = Resource?.Reduce() ?? false;

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
            var r = new UsingStatement(Resource);
            if (ShouldSerializeStatements())
            {
                r.Statements.AddRange(_Statements.Select(s => s.Clone()));
            }
            return r;
        }
    }
}