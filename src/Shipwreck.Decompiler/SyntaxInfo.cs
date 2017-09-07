using System.Collections.Generic;

namespace Shipwreck.Decompiler
{
    internal sealed class SyntaxInfo
    {
        public int? Offset { get; set; }

        public HashSet<object> From { get; set; }
        public HashSet<object> To { get; set; }
    }
}