namespace Shipwreck.Decompiler.Statements
{
    public sealed partial class ContinueStatement : Statement
    {
        public override bool IsEquivalentTo(Syntax other)
            => other is ContinueStatement;

        public override Statement Clone()
            => new ContinueStatement();
    }
}