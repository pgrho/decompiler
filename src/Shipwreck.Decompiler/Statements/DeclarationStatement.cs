using System;
using System.CodeDom.Compiler;

namespace Shipwreck.Decompiler.Statements
{
    public abstract class DeclarationStatement : Statement
    {
        public Type Type { get; set; }

        private VariableDeclaratorCollection _Declarators;

        public VariableDeclaratorCollection Declarators
            => _Declarators ?? (_Declarators = new VariableDeclaratorCollection());

        public bool ShouldSerializeDeclarators()
            => _Declarators?.Count > 0;

        public override bool IsEquivalentTo(Syntax other)
        {
            if (other == this)
            {
                return true;
            }
            if (other is DeclarationStatement ss
                && other.GetType() == GetType()
                && (_Declarators?.Count ?? 0) == (ss._Declarators?.Count ?? 0))
            {
                if (ShouldSerializeDeclarators())
                {
                    for (int i = 0; i < _Declarators.Count; i++)
                    {
                        var d = _Declarators[i];
                        var od = ss._Declarators[i];
                        if (d.Identifier != od.Identifier
                            || !(d.Initializer?.IsEquivalentTo(od.Initializer) ?? od.Initializer == null))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            return false;
        }

        public override bool Reduce()
        {
            if (!ShouldSerializeDeclarators())
            {
                if (Collection == null)
                {
                    return false;
                }
                Collection.Remove(this);
                return true;
            }

            var reduced = false;
            foreach (var d in Declarators)
            {
                if (d.Initializer != null && d.Initializer.TryReduce(out var e))
                {
                    d.Initializer = e;
                    reduced = true;
                }
            }
            return reduced;
        }

        public override void WriteTo(IndentedTextWriter writer)
        {
            if (ShouldSerializeDeclarators())
            {
                WriteDeclaration(writer);

                writer.WriteLine(';');
            }
        }

        internal void WriteDeclaration(IndentedTextWriter writer)
        {
            WriteTypeTo(writer);

            for (int i = 0; i < Declarators.Count; i++)
            {
                if (i > 0)
                {
                    writer.Write(", ");
                }
                var d = Declarators[i];

                writer.Write(d.Identifier);
                if (d.Initializer == null)
                {
                    writer.Write(" = ");
                    d.Initializer.WriteTo(writer);
                }
            }
        }

        internal abstract void WriteTypeTo(IndentedTextWriter writer);
    }

}