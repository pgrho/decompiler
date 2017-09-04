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
                case BinaryOperator.Modulo:
                case BinaryOperator.And:
                case BinaryOperator.Or:
                case BinaryOperator.ExclusiveOr:
                case BinaryOperator.LeftShift:
                case BinaryOperator.RightShift:
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

                case BinaryOperator.Modulo:
                    return "%";

                case BinaryOperator.And:
                    return "&";

                case BinaryOperator.Or:
                    return "|";

                case BinaryOperator.ExclusiveOr:
                    return "^";

                case BinaryOperator.LeftShift:
                    return "<<";

                case BinaryOperator.RightShift:
                    return ">>";

                case BinaryOperator.AndAlso:
                    return "&&";

                case BinaryOperator.OrElse:
                    return "||";

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