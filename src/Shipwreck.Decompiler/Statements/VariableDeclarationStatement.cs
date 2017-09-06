using System.CodeDom.Compiler;

namespace Shipwreck.Decompiler.Statements
{
    public abstract class VariableDeclarationStatement : DeclarationStatement
    {
        internal override void WriteTypeTo(IndentedTextWriter writer)
        {
            writer.Write(Type?.FullName ?? "var");
            writer.Write(' ');
        }
    }
}