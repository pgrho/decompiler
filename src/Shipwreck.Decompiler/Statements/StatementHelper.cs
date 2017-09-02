namespace Shipwreck.Decompiler.Statements
{
    public static class StatementHelper
    {
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
    }
}