using System.Collections.Generic;
using System.Linq;
using Shipwreck.CSharpModels.Expressions;

namespace Shipwreck.CSharpModels.Statements
{
    public sealed partial class SwitchStatement : Statement, IBreakableStatement
    {
        public SwitchStatement()
        {
            Expression = 0.ToExpression();
        }

        public SwitchStatement(Expression expression)
        {
            Expression = expression ?? 0.ToExpression();
        }

        public Expression Expression { get; set; }

        #region Sections

        private SwitchSectionCollection _Sections;

        public SwitchSectionCollection Sections
            => _Sections ?? (_Sections = new SwitchSectionCollection(this));

        public bool ShouldSerializeSections()
            => _Sections?.Count > 0;

        #endregion Sections

        public override Statement Clone()
        {
            var r = new SwitchStatement(Expression);

            if (ShouldSerializeSections())
            {
                foreach (var s in Sections)
                {
                    var ns = new SwitchSection();
                    foreach (var v in s.Labels)
                    {
                        ns.Labels.Add(v);
                    }
                    foreach (var v in s.Statements)
                    {
                        ns.Statements.Add(v.Clone());
                    }
                    r.Sections.Add(ns);
                }
            }
            return r;
        }

        public override bool IsEqualTo(Syntax other)
        {
            if (other == this)
            {
                return true;
            }
            if (other is SwitchStatement ss
                && Expression.IsEqualTo(ss.Expression)
                && (_Sections?.Count ?? 0) == (ss._Sections?.Count ?? 0))
            {
                if (ShouldSerializeSections())
                {
                    for (int i = 0; i < Sections.Count; i++)
                    {
                        var ms = Sections[i];
                        var os = ss.Sections[i];

                        if (ms.Labels.Count != os.Labels.Count)
                        {
                            return false;
                        }
                        for (int j = 0; j < ms.Labels.Count; j++)
                        {
                            var mv = ms.Labels[j];
                            var ov = os.Labels[j];

                            if (!(mv?.IsEqualTo(ov) ?? ov == null))
                            {
                                return false;
                            }
                        }

                        if (!ms.Statements.IsEqualTo(os.Statements))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            return false;
        }

        public override IEnumerable<StatementCollection> GetChildCollections()
        {
            if (ShouldSerializeSections())
            {
                foreach (var s in Sections)
                {
                    if (s.ShouldSerializeStatements())
                    {
                        yield return s.Statements;
                    }
                }
            }
        }

        public override bool Reduce()
        {
            var thisReduced = Expression.TryReduce(out var e);
            Expression = e;

            if (ShouldSerializeSections())
            {
                for (var i = 0; i < Sections.Count; i++)
                {
                    var s = Sections[i];

                    if (s.ShouldSerializeLabels() && s.ShouldSerializeStatements())
                    {
                        var r = s.Statements.ReduceBlock();
                        thisReduced |= r;
                    }
                    else
                    {
                        Sections.RemoveAt(i--);
                        thisReduced = true;
                    }
                }

                if (Expression is BinaryExpression be
                    && (be.Operator == BinaryOperator.Add || be.Operator == BinaryOperator.Subtract)
                    && be.Right is ConstantExpression ce)
                {
                    Expression = be.Left;
                    var op = be.Operator == BinaryOperator.Add ? BinaryOperator.Subtract : BinaryOperator.Add;

                    foreach (var s in Sections)
                    {
                        for (var i = 0; i < s.Labels.Count; i++)
                        {
                            s.Labels[i] = s.Labels[i]?.MakeBinary(be.Right, op).Reduce();
                        }
                    }

                    thisReduced = true;
                }
            }
            else
            {
                // TODO: to ExpressionStatement
            }

            if (Collection != null)
            {
                var i = Collection.IndexOf(this);

                thisReduced |= IncludeSectionBody(i)
                                | IntroduceDefaultSection(i);

                // TODO: remove unreachable cases
            }

            return thisReduced;
        }

        private bool IncludeSectionBody(int myIndex)
        {
            for (var labelIndex = myIndex + 1; labelIndex < Collection.Count; labelIndex++)
            {
                if (Collection[labelIndex] is LabelTarget lb
                    && Collection[labelIndex - 1] is GoToStatement gt)
                {
                    if (gt.Target == lb)
                    {
                        Collection.RemoveAt(labelIndex - 1);
                        return true;
                    }
                    else
                    {
                        var refs = lb.ReferencedFrom().ToArray();

                        if (refs.Length == 0)
                        {
                            Collection.RemoveAt(labelIndex);
                            return true;
                        }
                        else if (refs.Length == 1 && refs[0].Collection?.Owner == this)
                        {
                            for (var i = labelIndex + 1; i < Collection.Count; i++)
                            {
                                var s = Collection[i];

                                if (s is GoToStatement
                                    || s is ReturnStatement) // TODO: add throw
                                {
                                    // TODO: consider break statement
                                    // TODO: if and switch

                                    var sts = Collection.Skip(labelIndex + 1).Take(i - labelIndex).ToArray();
                                    Collection.RemoveRange(labelIndex, sts.Length + 1);

                                    refs[0].ReplaceBy(sts);

                                    return true;
                                }
                                else if (s.HasLabel())
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool IntroduceDefaultSection(int i)
        {
            var tgs = Sections.Select(s => (s.Statements.LastOrDefault() as GoToStatement)?.Target).Distinct().ToArray();

            if (tgs.Length == 1)
            {
                var j = Collection.IndexOf(tgs[0]);

                if (j > i)
                {
                    var body = Collection.Skip(i + 1).Take(j - i - 1);
                    if (!body.HaveLabel())
                    {
                        var sts = body.ToArray();
                        Collection.RemoveRange(i + 1, sts.Length);

                        foreach (var s in Sections)
                        {
                            s.Statements.RemoveAt(s.Statements.Count - 1);
                            s.Statements.Add(new BreakStatement());
                        }

                        var def = new SwitchSection();
                        def.Labels.Add(null);
                        def.Statements.AddRange(sts);
                        def.Statements.Add(new BreakStatement());

                        Sections.Add(def);

                        return true;
                    }
                }
            }
            return false;
        }
    }
}