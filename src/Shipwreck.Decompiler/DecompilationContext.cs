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
            RootStatements = new List<object>();
        }

        #region SyntaxInfo Methods

        private readonly ConditionalWeakTable<object, SyntaxInfo> _Infos = new ConditionalWeakTable<object, SyntaxInfo>();

        private SyntaxInfo GetInfo(object syntax)
            => _Infos.GetOrCreateValue(syntax);

        public int? GetOffset(int rootIndex)
            => GetInfo(RootStatements[rootIndex]).Offset;

        public int? GetOffset(object syntax)
            => GetInfo(syntax).Offset;

        public void SetOffset(object syntax, int? value)
            => GetInfo(syntax).Offset = value;

        public int GetToCount(object syntax)
            => GetInfo(syntax).To?.Count ?? 0;

        public IEnumerable<object> GetTo(object syntax)
            => GetInfo(syntax).To ?? Enumerable.Empty<object>();

        public int GetFromCount(object syntax)
            => GetInfo(syntax).From?.Count ?? 0;

        public IEnumerable<object> GetFrom(Syntax syntax)
            => GetInfo(syntax).From ?? Enumerable.Empty<object>();

        public object GetSyntaxAt(int offset)
            => RootStatements.FirstOrDefault(s => GetOffset(s) >= offset);

        public void SetTo(object from, object to)
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
                (fi.To ?? (fi.To = new HashSet<object>())).Add(to);

                var ti = GetInfo(to);
                (ti.From ?? (ti.From = new HashSet<object>())).Add(from);
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
                (fi.To ?? (fi.To = new HashSet<object>())).UnionWith(to);

                foreach (var t in to)
                {
                    var ti = GetInfo(t);
                    (ti.From ?? (ti.From = new HashSet<object>())).Add(from);
                }
            }
        }

        public void ClearTo(object syntax)
            => SetTo(syntax, (object)null);

        public void ReplaceInstructionFlow(object newNode, params object[] oldNodes)
            => ReplaceInstructionFlow(newNode, (IEnumerable<object>)oldNodes);

        public void ReplaceInstructionFlow(object newNode, IEnumerable<object> oldNodes)
        {
            var removing = oldNodes as IReadOnlyCollection<object> ?? oldNodes.ToList();

            var froms = removing.SelectMany(r => GetInfo(r).From ?? Enumerable.Empty<object>()).Distinct().Except(removing).ToList();
            var tos = removing.SelectMany(r => GetInfo(r).To ?? Enumerable.Empty<object>()).Distinct().Except(removing).ToList();

            foreach (var r in removing)
            {
                ClearTo(r);
            }

            if (newNode != null)
            {

                foreach (var f in froms)
                {
                    SetTo(f, GetTo(f).Except(removing).Union(new[] { newNode }));
                }

                SetTo(newNode, tos);
            }
            else
            {
                var to = tos.SingleOrDefault();

                foreach (var f in froms)
                {
                    if (to != null)
                    {
                        SetTo(f, GetTo(f).Except(removing).Union(new[] { to }));
                    }
                    else
                    {
                        SetTo(f, GetTo(f).Except(removing));
                    }
                }
            }
        }
        public void ReplaceInstructionFlow(IEnumerable<object> newNodes, params object[] oldNodes)
            => ReplaceInstructionFlow(newNodes, (IEnumerable<object>)oldNodes);

        public void ReplaceInstructionFlow(IEnumerable<object> newNodes, IEnumerable<object> oldNodes)
        {
            var removing = oldNodes as IReadOnlyCollection<object> ?? oldNodes.ToList();

            var nf = newNodes.First();
            var nl = newNodes.Last();

            var froms = removing.SelectMany(r => GetInfo(r).From ?? Enumerable.Empty<object>()).Distinct().Except(removing).ToList();
            var tos = removing.SelectMany(r => GetInfo(r).To ?? Enumerable.Empty<object>()).Distinct().Except(removing).ToList();

            foreach (var r in removing)
            {
                ClearTo(r);
            }

            foreach (var f in froms)
            {
                SetTo(f, GetTo(f).Except(removing).Union(new[] { nf }));
            }

            SetTo(nl, tos);
        }

        #endregion SyntaxInfo Methods

        public MethodBase Method { get; }

        public List<object> RootStatements { get; }

        private ThisExpression _This;

        public ThisExpression This
            => _This ?? (_This = new ThisExpression(Method.DeclaringType));

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