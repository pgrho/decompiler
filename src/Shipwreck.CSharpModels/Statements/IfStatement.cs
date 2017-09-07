using System;
using System.Collections.Generic;
using System.Linq;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed partial class IfStatement : Statement
    {
        public IfStatement()
        {
            Condition = ExpressionBuilder.False;
        }

        public IfStatement(Expression condition)
        {
            Condition = condition ?? ExpressionBuilder.False;
        }

        public Expression Condition { get; set; }

        #region TruePart

        private StatementCollection _TruePart;

        public StatementCollection TruePart
            => _TruePart ?? (_TruePart = new StatementCollection(this));

        public bool ShouldSerializeTruePart()
            => _TruePart.ShouldSerialize();

        #endregion TruePart

        #region FalsePart

        private StatementCollection _FalsePart;

        public StatementCollection FalsePart
            => _FalsePart ?? (_FalsePart = new StatementCollection(this));

        public bool ShouldSerializeFalsePart()
            => _FalsePart.ShouldSerialize();

        #endregion FalsePart

        public override bool IsEqualTo(Syntax other)
            => this == (object)other
            || (other is IfStatement gt
                && Condition.IsEqualTo(gt.Condition)
                && _TruePart.IsEqualTo(gt._TruePart)
                && _FalsePart.IsEqualTo(gt._FalsePart));

        public override bool Reduce()
        {
            var thisReduced = Condition.TryReduce(out var nc);
            if (thisReduced)
            {
                Condition = nc;
            }

            if (Collection != null)
            {
                if (Condition is ConstantExpression ce)
                {
                    var i = Collection.IndexOf(this);

                    var block = ce.Value == null || Activator.CreateInstance(ce.GetType()).Equals(ce.Value) ? _FalsePart : _TruePart;

                    Collection.RemoveAt(i);
                    if (block.ShouldSerialize())
                    {
                        var items = block.ToArray();
                        Collection.InsertRange(i, items);
                    }
                    return true;
                }
                else
                {
                    var i = Collection.IndexOf(this);
                    if (i > 0
                        && Collection[i - 1] is ExpressionStatement es
                        && es.Expression is AssignmentExpression ae
                        && Condition.TryReplace(ae.Left, ae, out var replaced))
                    {
                        Collection.RemoveAt(i - 1);
                        Condition = replaced;

                        return true;
                    }
                }

                bool iterReduced;
                do
                {
                    if (Collection == null)
                    {
                        return true;
                    }

                    iterReduced = false;
                    if (_TruePart != null)
                    {
                        var r = _TruePart.ReduceBlock() | ReduceLastGoto(_TruePart);
                        thisReduced |= r;
                        iterReduced |= r;
                    }
                    if (_FalsePart != null)
                    {
                        var r = _FalsePart.ReduceBlock() | ReduceLastGoto(_FalsePart);
                        thisReduced |= r;
                        iterReduced |= r;
                    }
                } while (iterReduced);

                if (!ShouldSerializeTruePart())
                {
                    if (ShouldSerializeFalsePart())
                    {
                        var items = FalsePart.ToArray();
                        FalsePart.Clear();
                        TruePart.AddRange(items);
                        Condition = Condition.LogicalNot();
                    }
                    else
                    {
                        Collection.Remove(this);
                    }
                    return true;
                }
            }

            return thisReduced;
        }

        private bool ReduceLastGoto(StatementCollection block)
        {
            if (Collection == null)
            {
                return false;
            }
            var gt = block.LastOrDefault() as GoToStatement;

            if (gt != null)
            {
                var j = Collection.IndexOf(gt.Target);

                if (j >= 0)
                {
                    var i = Collection.IndexOf(this);

                    if (j < i)
                    {
                        if (block.Count == 1)
                        {
                            Expression c;
                            if (block == _TruePart && !ShouldSerializeFalsePart())
                            {
                                c = Condition;
                            }
                            else if (block == _FalsePart && !ShouldSerializeTruePart())
                            {
                                c = Condition.LogicalNot().Reduce();
                            }
                            else
                            {
                                return false;
                            }

                            var cls = Collection;
                            var sts = cls.Skip(j + 1).Take(i - j - 1).ToArray();
                            cls.RemoveRange(j + 1, sts.Length + 1);
                            var dw = new DoWhileStatement(c);
                            dw.Statements.AddRange(sts);

                            cls.Insert(j + 1, dw);
                            return true;
                        }
                    }
                    else if (i + 1 == j)
                    {
                        block.RemoveAt(block.Count - 1);

                        return true;
                    }
                    else
                    {
                        var mid = Collection.Skip(i + 1).Take(j - i - 1);
                        if (!mid.OfType<LabelTarget>().Any())
                        {
                            var m = mid.ToArray();
                            Collection.RemoveRange(i + 1, j - i - 1);
                            (block == _TruePart ? FalsePart : TruePart).AddRange(m);

                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override IEnumerable<StatementCollection> GetChildCollections()
        {
            if (ShouldSerializeTruePart())
            {
                yield return _TruePart;
            }
            if (ShouldSerializeFalsePart())
            {
                yield return _FalsePart;
            }
        }

        public override Statement Clone()
        {
            var r = new IfStatement(Condition);
            if (ShouldSerializeTruePart())
            {
                r.TruePart.AddRange(_TruePart.Select(s => s.Clone()));
            }
            if (ShouldSerializeFalsePart())
            {
                r.FalsePart.AddRange(_FalsePart.Select(s => s.Clone()));
            }
            return r;
        }
    }
}