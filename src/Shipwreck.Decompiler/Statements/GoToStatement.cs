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
                var j = Collection.IndexOf(Target);

                if (j >= 0)
                {
                    if (j < i)
                    {
                        var items = Collection.Skip(j + 1).Take(i - j - 1).ToArray();
                        Collection.RemoveRange(j + 1, i - j - 1);

                        var w = new WhileStatement();
                        w.Statements.AddRange(items);

                        Collection[j + 1] = w;
                        return true;
                    }
                    else if (j == i + 1)
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
                else
                {
                    var cs = this.Ancestors().OfType<IContinuableStatement>().FirstOrDefault();

                    if (cs != null && cs.ShouldSerializeStatements())
                    {
                        if (cs.Statements.LastOrDefault() == Target)
                        {
                            Collection[i] = new ContinueStatement();
                            return true;
                        }
                    }
                }
            }

            return base.Reduce();
        }
    }
}