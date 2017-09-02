using System.CodeDom.Compiler;

namespace Shipwreck.Decompiler.Statements
{
    internal sealed class TemporalGoToStatement : Statement
    {
        public TemporalGoToStatement(int target)
        {
            Target = target;
        }

        public int Target { get; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is TemporalGoToStatement gt && Target == gt.Target);

        public override void WriteTo(IndentedTextWriter writer)
        {
            writer.Write("goto L_");
            writer.Write(Target.ToString("x4"));
            writer.WriteLine(';');
        }
    }
}