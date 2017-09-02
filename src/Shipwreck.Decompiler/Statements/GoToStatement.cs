using System.CodeDom.Compiler;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class GoToStatement : Statement
    {
        public GoToStatement(LabelTarget target)
        {
            target.ArgumentIsNotNull(nameof(target));

            Target = target;
        }

        public LabelTarget Target { get; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is GoToStatement gt && Target == gt.Target);

        public override void WriteTo(IndentedTextWriter writer)
        {
            writer.Write("goto ");
            writer.Write(Target.Name);
            writer.WriteLine(';');
        }
    }
}