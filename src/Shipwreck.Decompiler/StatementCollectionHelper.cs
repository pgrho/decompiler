namespace Shipwreck.Decompiler
{
    internal static class StatementCollectionHelper
    {
        public static bool ShouldSerialize(this StatementCollection collection)
            => collection?.Count > 0;
    }
}