namespace Shipwreck.Decompiler.Statements
{
    public sealed partial class ContinueStatement : Statement
    {
        public override bool IsEqualTo(Syntax other)
            => other is ContinueStatement;

        public override Statement Clone()
            => new ContinueStatement();
    }
}