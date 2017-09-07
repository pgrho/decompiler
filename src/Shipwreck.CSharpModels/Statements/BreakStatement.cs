namespace Shipwreck.Decompiler.Statements
{
    public sealed partial class BreakStatement : Statement, IBreakingStatement
    {
        public override bool IsEqualTo(Syntax other)
            => other is BreakStatement;

        public override Statement Clone()
            => new BreakStatement();
    }
}