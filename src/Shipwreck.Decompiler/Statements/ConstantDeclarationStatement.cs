using System;
using System.CodeDom.Compiler;

namespace Shipwreck.Decompiler.Statements
{
    public sealed partial class ConstantDeclarationStatement : DeclarationStatement
    {
        internal override void WriteTypeTo(IndentedTextWriter writer)
        {
            writer.Write("const ");
            writer.Write(Type.FullName);
            writer.Write(' ');
        }

        public override Statement Clone()
        {
            var r = new ConstantDeclarationStatement();
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