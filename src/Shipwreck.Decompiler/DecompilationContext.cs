using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler
{
    internal sealed class DecompilationContext
    {
        public DecompilationContext(MethodBase method)
        {
            Method = method;
            RootStatements = new List<Syntax>();
        }

        #region SyntaxInfo Methods

        private readonly ConditionalWeakTable<Syntax, SyntaxInfo> _Infos = new ConditionalWeakTable<Syntax, SyntaxInfo>();

        private SyntaxInfo GetInfo(Syntax syntax)
            => _Infos.GetOrCreateValue(syntax);

        public int? GetOffset(int rootIndex)
            => GetInfo(RootStatements[rootIndex]).Offset;

        public int? GetOffset(Syntax syntax)
            => GetInfo(syntax).Offset;

        public void SetOffset(Syntax syntax, int? value)
            => GetInfo(syntax).Offset = value;

        public int GetToCount(Syntax syntax)
            => GetInfo(syntax).To?.Count ?? 0;

        public IEnumerable<Syntax> GetTo(Syntax syntax)
            => GetInfo(syntax).To ?? Enumerable.Empty<Syntax>();

        public int GetFromCount(Syntax syntax)
            => GetInfo(syntax).From?.Count ?? 0;

        public IEnumerable<Syntax> GetFrom(Syntax syntax)
            => GetInfo(syntax).From ?? Enumerable.Empty<Syntax>();

        public Syntax GetSyntaxAt(int offset)
            => RootStatements.FirstOrDefault(s => GetOffset(s) >= offset);

        public void SetTo(Syntax from, Syntax to)
        {
            var fi = GetInfo(from);

            if (fi.To != null)
            {
                foreach (var currentTo in fi.To)
                {
                    var cti = GetInfo(currentTo);
                    cti.From?.Remove(from);
                }

                fi.To.Clear();
            }

            if (to != null)
            {
                (fi.To ?? (fi.To = new HashSet<Syntax>())).Add(to);

                var ti = GetInfo(to);
                (ti.From ?? (ti.From = new HashSet<Syntax>())).Add(from);
            }
        }

        public void SetTo(Syntax from, IEnumerable<Syntax> to)
        {
            var fi = GetInfo(from);

            if (fi.To != null)
            {
                foreach (var currentTo in fi.To)
                {
                    var cti = GetInfo(currentTo);
                    cti.From?.Remove(from);
                }

                fi.To.Clear();
            }

            if (to != null)
            {
                (fi.To ?? (fi.To = new HashSet<Syntax>())).UnionWith(to);

                foreach (var t in to)
                {
                    var ti = GetInfo(t);
                    (ti.From ?? (ti.From = new HashSet<Syntax>())).Add(from);
                }
            }
        }

        public void ClearTo(Syntax syntax)
            => SetTo(syntax, (Syntax)null);

        public void ReplaceInstructionFlow(Syntax newNode, IEnumerable<Syntax> oldNodes)
        {
            var removing = oldNodes as IReadOnlyCollection<Syntax> ?? oldNodes.ToList();

            var froms = removing.SelectMany(r => GetInfo(r).From ?? Enumerable.Empty<Syntax>()).Distinct().Except(removing).ToList();
            var tos = removing.SelectMany(r => GetInfo(r).To ?? Enumerable.Empty<Syntax>()).Distinct().Except(removing).ToList();

            foreach (var r in removing)
            {
                ClearTo(r);
            }

            foreach (var f in froms)
            {
                SetTo(f, GetTo(f).Except(removing).Union(new[] { newNode }));
            }

            SetTo(newNode, tos);
        }

        #endregion SyntaxInfo Methods

        public MethodBase Method { get; }

        public List<Syntax> RootStatements { get; }

        private ThisExpression _This;

        public ThisExpression This
            => _This ?? (_This = new ThisExpression());

        private ParameterExpression[] _Parameters;

        public Expression GetParameter(int index)
        {
            var @params = Method.GetParameters();
            if (_Parameters == null)
            {
                _Parameters = new ParameterExpression[@params.Length];
            }

            return _Parameters[index] ?? (_Parameters[index] = new ParameterExpression(@params[index]));
        }
    }
}