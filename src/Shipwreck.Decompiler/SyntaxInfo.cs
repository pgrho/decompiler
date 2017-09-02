using System.Collections.Generic;

namespace Shipwreck.Decompiler
{
    internal sealed class SyntaxInfo
    {
        public int? Offset { get; set; }

        public HashSet<Syntax> From { get; set; }
        public HashSet<Syntax> To { get; set; }
    }
}