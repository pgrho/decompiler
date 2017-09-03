using System;

namespace Shipwreck.Decompiler.Expressions
{
    internal static class BinaryOperatorHelper
    {
        public static bool IsChecked(this BinaryOperator @operator)
            => @operator == BinaryOperator.AddChecked
            || @operator == BinaryOperator.SubtractChecked
            || @operator == BinaryOperator.MultiplyChecked;

        public static bool CanAssign(this BinaryOperator @operator)
        {
            switch (@operator)
            {
                case BinaryOperator.Default:
                case BinaryOperator.Add:
                case BinaryOperator.AddChecked:
                case BinaryOperator.Subtract:
                case BinaryOperator.SubtractChecked:
                case BinaryOperator.Multiply:
                case BinaryOperator.MultiplyChecked:
                case BinaryOperator.Divide:
                    return true;
            }
            return false;
        }

        public static string GetToken(this BinaryOperator @operator)
        {
            switch (@operator)
            {
                case BinaryOperator.Add:
                case BinaryOperator.AddChecked:
                    return "+";

                case BinaryOperator.Subtract:
                case BinaryOperator.SubtractChecked:
                    return "-";

                case BinaryOperator.Multiply:
                case BinaryOperator.MultiplyChecked:
                    return "*";

                case BinaryOperator.Divide:
                    return "/";

                case BinaryOperator.Equal:
                    return "==";

                case BinaryOperator.NotEqual:
                    return "!=";

                case BinaryOperator.LessThan:
                    return "<";

                case BinaryOperator.LessThanOrEqual:
                    return "<=";

                case BinaryOperator.GreaterThan:
                    return ">";

                case BinaryOperator.GreaterThanOrEqual:
                    return ">=";

                default:
                    throw new ArgumentException();
            }
        }
    }
}