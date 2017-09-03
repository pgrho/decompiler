using System.CodeDom.Compiler;
using System.Linq;

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

        public override bool Reduce()
        {
            if (Collection != null)
            {
                var i = Collection.IndexOf(this);

                if (Collection.ElementAtOrDefault(i + 1) == Target)
                {
                    if (Target.ReferencedFrom().Count() < 2)
                    {
                        Collection.RemoveRange(i, 2);
                    }
                    else
                    {
                        Collection.Remove(this);
                    }

                    return true;
                }
            }

            return base.Reduce();
        }
    }
}