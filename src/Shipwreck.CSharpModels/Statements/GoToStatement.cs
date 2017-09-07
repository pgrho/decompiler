using System.Linq;

namespace Shipwreck.CSharpModels.Statements
{
    public sealed partial class GoToStatement : Statement, IBreakingStatement
    {
        public GoToStatement(LabelTarget target)
        {
            target.ArgumentIsNotNull(nameof(target));

            Target = target;
        }

        public LabelTarget Target { get; }

        public override bool IsEqualTo(Syntax other)
            => this == (object)other
            || (other is GoToStatement gt && Target == gt.Target);

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
                    var isLast = i == Collection.Count - 1;
                    var continuable = true;
                    var breakable = true;
                    var ans = Collection.Owner as Statement;

                    // TODO: consider finally block
                    while ((isLast || continuable || breakable)
                        && ans?.Collection != null)
                    {
                        var inBreakable = ans.Collection.Owner is IBreakableStatement;
                        var inLoop = ans.Collection.Owner is IIterationStatement;

                        var ai = ans.Collection.IndexOf(ans);
                        var k = ans.Collection.IndexOf(Target);

                        if (k >= 0)
                        {
                            if (k == ai + 1)
                            {
                                if (isLast && !inLoop)
                                {
                                    Collection.RemoveAt(i);
                                    return true;
                                }
                                if (breakable && inBreakable)
                                {
                                    Collection[i] = new BreakStatement();
                                    return true;
                                }
                            }
                            else if (k == ans.Collection.Count - 1)
                            {
                                if (continuable && inLoop)
                                {
                                    Collection[i] = new ContinueStatement();
                                    return true;
                                }
                            }
                        }

                        isLast &= ai == ans.Collection.Count - 1;
                        continuable &= !inLoop;
                        breakable &= !inBreakable;

                        ans = ans.Collection.Owner as Statement;
                    }
                }
            }

            return base.Reduce();
        }

        public override Statement Clone()
            => new GoToStatement(Target);
    }
}