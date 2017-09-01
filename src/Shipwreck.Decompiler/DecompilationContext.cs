using System.Collections.Generic;
using System.Reflection;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler
{
    internal sealed class DecompilationContext
    {
        public DecompilationContext(MethodBase method)
        {
            Method = method;
            Flow = new List<SyntaxContainer>();
        }

        public MethodBase Method { get; }

        public List<SyntaxContainer> Flow { get; }

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