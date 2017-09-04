namespace Shipwreck.Decompiler
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
    }
}