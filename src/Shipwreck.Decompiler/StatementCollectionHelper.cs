using System.Linq;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler
{
    internal static class StatementCollectionHelper
    {
        public static bool ShouldSerialize(this StatementCollection collection)
            => collection?.Count > 0;

        public static bool ReduceBlock(this StatementCollection block)
        {
            if (block == null)
            {
                return false;
            }

            foreach (var s in block)
            {
                if (s.Reduce())
                {
                    return true;
                }
            }

            var gt = block.LastOrDefault() as GoToStatement;

            if (gt != null)
            {
                var ancestor = gt.Collection.Owner as Statement;

                while (ancestor?.Collection != null)
                {
                    var i = ancestor.Collection.IndexOf(ancestor);

                    if (0 <= i && i + 1 < ancestor.Collection.Count && gt.Target == ancestor.Collection[i + 1])
                    {
                        block.RemoveAt(block.Count - 1);
                        return true;
                    }

                    ancestor = ancestor.Collection.Owner as Statement;
                }
            }
            return false;
        }
    }
}