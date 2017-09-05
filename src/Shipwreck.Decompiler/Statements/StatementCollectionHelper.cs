using System.Collections.Generic;

namespace Shipwreck.Decompiler.Statements
{
    internal static class StatementCollectionHelper
    {
        public static bool ShouldSerialize(this StatementCollection collection)
            => collection?.Count > 0;

        public static bool ReduceBlock(this StatementCollection block)
        {
            if (block != null)
            {
                foreach (var s in block)
                {
                    if (s.Reduce())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static Statement GetPreviousOf(this StatementCollection collection, Statement statement, out int i)
        {
            i = collection?.IndexOf(statement) ?? -1;
            if (i > 0)
            {
                return collection[i - 1];
            }
            return null;
        }

        public static bool IsEquivalentTo(this StatementCollection collection, StatementCollection other)
        {
            if (collection == null)
            {
                return !(other?.Count > 0);
            }
            else if (other?.Count != collection.Count)
            {
                return false;
            }

            for (int i = 0; i < collection.Count; i++)
            {
                if (!collection[i].IsEquivalentTo(other[i]))
                {
                    return false;
                }
            }
            return true;
        }

        internal static bool HaveLabel(this IEnumerable<Statement> statements)
        {
            foreach (var s in statements)
            {
                if (s.HasLabel())
                {
                    return true;
                }
            }
            return false;
        }
    }
}