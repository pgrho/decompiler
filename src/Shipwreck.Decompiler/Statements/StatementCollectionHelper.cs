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
                var reduced = false;
                bool iter;
                do
                {
                    iter = false;
                    for (var i = 0; i < block.Count; i++)
                    {
                        var s = block[i];

                        if (s.Reduce())
                        {
                            reduced = iter = true;
                            break;
                        }
                        else if (s.IsBreaking())
                        {
                            while (i + 1 < block.Count)
                            {
                                var ns = block[i + 1];
                                if (ns.HasLabel())
                                {
                                    break;
                                }

                                block.RemoveAt(i + 1);
                                reduced = iter = true;
                            }
                            if (iter)
                            {
                                break;
                            }
                        }
                    }
                } while (iter);

                return reduced;
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