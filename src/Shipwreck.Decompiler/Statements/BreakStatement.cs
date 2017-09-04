using System.CodeDom.Compiler;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class BreakStatement : Statement
    {
        public override bool IsEquivalentTo(Syntax other)
            => other is BreakStatement;

        public override void WriteTo(IndentedTextWriter writer)
            => writer.WriteLine("break;");


        public override Statement Clone()
            => new BreakStatement();
    }
}