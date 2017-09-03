using System.CodeDom.Compiler;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class ContinueStatement : Statement
    {
        public override bool IsEquivalentTo(Syntax other)
            => other is ContinueStatement;

        public override void WriteTo(IndentedTextWriter writer)
            => writer.WriteLine("continue;");
    }
}