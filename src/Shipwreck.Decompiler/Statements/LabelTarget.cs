using System.CodeDom.Compiler;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class LabelTarget : Statement
    {
        public LabelTarget(string name)
        {
            name.ArgumentIsNotNull(nameof(name));

            Name = name;
        }

        public string Name { get; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is LabelTarget lb && Name == lb.Name);

        public override void WriteTo(IndentedTextWriter writer)
        {
            writer.Write(Name);
            writer.Write(':');
        }
    }
}