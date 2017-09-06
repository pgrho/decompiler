using System.CodeDom.Compiler;

namespace Shipwreck.Decompiler.Statements
{
    public sealed partial class VariableDeclarationStatement : DeclarationStatement
    {
        internal override void WriteTypeTo(IndentedTextWriter writer)
        {
            writer.Write(Type?.FullName ?? "var");
            writer.Write(' ');
        }

        public override Statement Clone()
        {
            var r = new VariableDeclarationStatement();
            r.Type = Type;
            if (ShouldSerializeDeclarators())
            {
                foreach (var d in Declarators)
                {
                    r.Declarators.Add(new VariableDeclarator()
                    {
                        Identifier = d.Identifier,
                        Initializer = d.Initializer
                    });
                }
            }

            return r;
        }
    }
}