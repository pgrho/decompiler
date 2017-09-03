using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class IfBlock : Statement
    {
        public IfBlock()
        {
            Condition = ExpressionBuilder.False;
        }

        public IfBlock(Expression condition)
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

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is IfBlock gt
                && Condition.IsEquivalentTo(gt.Condition)
                && _TruePart.IsEquivalentTo(gt._TruePart)
                && _FalsePart.IsEquivalentTo(gt._FalsePart));

        public override void WriteTo(IndentedTextWriter writer)
        {
            writer.Write("if (");
            Condition.WriteTo(writer);
            writer.WriteLine(')');

            writer.WriteLine('{');
            if (ShouldSerializeTruePart())
            {
                writer.Indent++;
                foreach (var s in _TruePart)
                {
                    s.WriteTo(writer);
                }
                writer.Indent--;
            }
            writer.WriteLine('}');

            if (ShouldSerializeFalsePart())
            {
                if (_FalsePart.Count == 1 && _FalsePart[0] is IfBlock cib)
                {
                    writer.Write("else ");
                    cib.WriteTo(writer);
                }
                else
                {
                    writer.WriteLine("else");
                    writer.WriteLine('{');
                    writer.Indent++;
                    foreach (var s in _FalsePart)
                    {
                        s.WriteTo(writer);
                    }
                    writer.Indent--;
                    writer.WriteLine('}');
                }
            }
        }

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

                bool iterReduced;
                do
                {
                    iterReduced = false;
                    if (_TruePart != null)
                    {
                        foreach (var s in _TruePart)
                        {
                            if (s.Reduce())
                            {
                                thisReduced = iterReduced = true;
                                break;
                            }
                        }

                        var r = ReduceLastGoto(_TruePart);
                        thisReduced |= r;
                        iterReduced |= r;
                    }
                    if (_FalsePart != null)
                    {
                        foreach (var s in _FalsePart)
                        {
                            if (s.Reduce())
                            {
                                thisReduced = iterReduced = true;
                                break;
                            }
                        }
                        var r = ReduceLastGoto(_FalsePart);
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
                        Condition = Condition.Negate();
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
            var gt = block.LastOrDefault() as GoToStatement;

            if (gt != null)
            {
                var j = Collection.IndexOf(gt.Target);

                if (j >= 0)
                {
                    var i = Collection.IndexOf(this);

                    if (i + 1 == j)
                    {
                        block.RemoveAt(block.Count - 1);

                        return true;
                    }
                    else if (i < j)
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
    }
}