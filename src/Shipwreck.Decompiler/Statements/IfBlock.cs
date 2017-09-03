using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class IfBlock : Statement
    {
        public IfBlock(Expression condition)
        {
            condition.ArgumentIsNotNull(nameof(condition));

            Condition = condition;
        }

        public Expression Condition { get; }

        #region TruePart

        private StatementCollection _TruePart;

        public StatementCollection TruePart
            => _TruePart ?? (_TruePart = new StatementCollection(this));

        #endregion TruePart

        #region FalsePart

        private StatementCollection _FalsePart;

        public StatementCollection FalsePart
            => _FalsePart ?? (_FalsePart = new StatementCollection(this));

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
            if (_TruePart?.Count > 0)
            {
                writer.Indent++;
                foreach (var s in _TruePart)
                {
                    s.WriteTo(writer);
                }
                writer.Indent--;
            }
            writer.WriteLine('}');

            if (_FalsePart?.Count > 0)
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
            if (Collection != null)
            {
                var reduced1 = false;
                bool reduced;
                do
                {
                    reduced = false;
                    if (_TruePart != null)
                    {
                        foreach (var s in _TruePart)
                        {
                            if (s.Reduce())
                            {
                                reduced1 = reduced = true;
                                break;
                            }
                        }
                    }
                    if (_FalsePart != null)
                    {
                        foreach (var s in _FalsePart)
                        {
                            if (s.Reduce())
                            {
                                reduced1 = reduced = true;
                                break;
                            }
                        }
                    }
                } while (reduced);

                if (!(_TruePart?.Count > 0) && !(_FalsePart?.Count > 0))
                {
                    Collection.Remove(this);
                    return true;
                }

                return reduced1;
            }

            return base.Reduce();
        }

        public override IEnumerable<StatementCollection> GetChildCollections()
        {
            if (_TruePart?.Count > 0)
            {
                yield return _TruePart;
            }
            if (_FalsePart?.Count > 0)
            {
                yield return _FalsePart;
            }
        }
    }
}