using System;

namespace Shipwreck.CSharpModels.Expressions
{
    public static class BinaryOperatorHelper
    {
        public static bool IsChecked(this BinaryOperator @operator)
            => @operator == BinaryOperator.AddChecked
            || @operator == BinaryOperator.SubtractChecked
            || @operator == BinaryOperator.MultiplyChecked;

        public static bool IsShift(this BinaryOperator @operator)
            => @operator == BinaryOperator.LeftShift
            || @operator == BinaryOperator.RightShift;

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

                case BinaryOperator.NullCoalesce:
                    return "??";

                default:
                    throw new ArgumentException();
            }
        }

        internal static string GetMethodName(this BinaryOperator o)
        {
            switch (o)
            {
                case BinaryOperator.Add:
                    return "op_Addition";

                case BinaryOperator.Subtract:
                    return "op_Subtraction";

                case BinaryOperator.Multiply:
                    return "op_Multiply";

                case BinaryOperator.Divide:
                    return "op_Division";

                case BinaryOperator.Modulo:
                    return "op_Modulus";

                case BinaryOperator.Equal:
                    return "op_Equality";

                case BinaryOperator.NotEqual:
                    return "op_Inequality";

                case BinaryOperator.GreaterThan:
                    return "op_GreaterThan";

                case BinaryOperator.GreaterThanOrEqual:
                    return "op_GreaterThanOrEqual";

                case BinaryOperator.LessThan:
                    return "op_LessThan";

                case BinaryOperator.LessThanOrEqual:
                    return "op_LessThanOrEqual";

                case BinaryOperator.And:
                    return "op_BitwiseAnd";

                case BinaryOperator.Or:
                    return "op_BitwiseOr";

                case BinaryOperator.ExclusiveOr:
                    return "op_ExclusiveOr";

                case BinaryOperator.LeftShift:
                    return "op_LeftShift";

                case BinaryOperator.RightShift:
                    return "op_RightShift";
            }
            return null;
        }

        internal static bool FromMethodName(string s, out BinaryOperator op)
        {
            switch (s)
            {
                case "op_Addition":
                    op = BinaryOperator.Add;
                    return true;

                case "op_Subtraction":
                    op = BinaryOperator.Subtract;
                    return true;

                case "op_Multiply":
                    op = BinaryOperator.Multiply;
                    return true;

                case "op_Division":
                    op = BinaryOperator.Divide;
                    return true;

                case "op_Modulus":
                    op = BinaryOperator.Modulo;
                    return true;

                case "op_Equality":
                    op = BinaryOperator.Equal;
                    return true;

                case "op_Inequality":
                    op = BinaryOperator.NotEqual;
                    return true;

                case "op_GreaterThan":
                    op = BinaryOperator.GreaterThan;
                    return true;

                case "op_GreaterThanOrEqual":
                    op = BinaryOperator.GreaterThanOrEqual;
                    return true;

                case "op_LessThan":
                    op = BinaryOperator.LessThan;
                    return true;

                case "op_LessThanOrEqual":
                    op = BinaryOperator.LessThanOrEqual;
                    return true;

                case "op_BitwiseAnd":
                    op = BinaryOperator.And;
                    return true;

                case "op_BitwiseOr":
                    op = BinaryOperator.Or;
                    return true;

                case "op_ExclusiveOr":
                    op = BinaryOperator.ExclusiveOr;
                    return true;

                case "op_LeftShift":
                    op = BinaryOperator.LeftShift;
                    return true;

                case "op_RightShift":
                    op = BinaryOperator.RightShift;
                    return true;
            }

            op = BinaryOperator.Default;
            return false;
        }
    }
}