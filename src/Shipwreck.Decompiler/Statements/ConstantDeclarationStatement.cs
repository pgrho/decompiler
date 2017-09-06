using System;
using System.CodeDom.Compiler;

namespace Shipwreck.Decompiler.Statements
{
    public abstract class ConstantDeclarationStatement : DeclarationStatement
    {
        internal override void WriteTypeTo(IndentedTextWriter writer)
        {
            writer.Write("const ");
            writer.Write(Type.FullName);
            writer.Write(' ');
        }
    }
}