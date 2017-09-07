namespace Shipwreck.Decompiler.Expressions
{
    internal static class UnaryOperatorHelper
    {
        public static bool IsConvert(this UnaryOperator v)
            => v == UnaryOperator.Convert
            || v == UnaryOperator.ConvertChecked
            || v == UnaryOperator.TypeAs;
    }
}