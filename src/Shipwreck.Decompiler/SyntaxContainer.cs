namespace Shipwreck.Decompiler
{
    public sealed class SyntaxContainer
    {
        public SyntaxContainer(int offset, Syntax syntax)
        {
            Offset = offset;
            Syntax = syntax;
        }

        public int Offset { get; }

        public Syntax Syntax { get; set; }
    }
}