using System.Collections.Generic;
using System.Reflection;

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
    }
}