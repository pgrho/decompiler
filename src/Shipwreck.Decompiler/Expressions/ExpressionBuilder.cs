using System;
using System.Collections.Generic;
using System.Reflection;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Expressions
{
    public static class ExpressionBuilder
    {
        #region Constant

        #region True

        private static ConstantExpression _True;

        internal static ConstantExpression True
            => _True ?? (_True = new ConstantExpression(true));

        #endregion True

        #region False

        private static ConstantExpression _False;

        internal static ConstantExpression False
            => _False ?? (_False = new ConstantExpression(false));

        #endregion False

        #endregion Constant

        public static ConstantExpression ToExpression<T>(this T value)
            => new ConstantExpression(value, typeof(T));

        #region UnaryExpression

        public static UnaryExpression MakeUnary(this Expression operand, UnaryOperator @operator)
            => new UnaryExpression(operand, @operator);

        public static UnaryExpression Negate(this Expression operand)
            => operand.MakeUnary(UnaryOperator.UnaryNegation);

        public static UnaryExpression LogicalNot(this Expression operand)
            => operand.MakeUnary(UnaryOperator.LogicalNot);

        public static UnaryExpression OnesComplement(this Expression operand)
            => operand.MakeUnary(UnaryOperator.OnesComplement);

        public static UnaryExpression PostIncrement(this Expression operand)
            => operand.MakeUnary(UnaryOperator.PostIncrement);

        public static UnaryExpression PostDecrement(this Expression operand)
            => operand.MakeUnary(UnaryOperator.PostDecrement);

        public static UnaryExpression PreIncrement(this Expression operand)
            => operand.MakeUnary(UnaryOperator.PreIncrement);

        public static UnaryExpression PreDecrement(this Expression operand)
            => operand.MakeUnary(UnaryOperator.PreDecrement);

        public static UnaryExpression AddressOf(this Expression operand)
            => operand.MakeUnary(UnaryOperator.AddressOf);

        public static UnaryExpression Convert(this Expression operand, Type type)
            => new UnaryExpression(operand, UnaryOperator.Convert, type);

        public static UnaryExpression ConvertChecked(this Expression operand, Type type)
            => new UnaryExpression(operand, UnaryOperator.ConvertChecked, type);

        #endregion UnaryExpression

        #region BinaryExpression

        public static BinaryExpression MakeBinary(this Expression left, Expression right, BinaryOperator @operator)
        {
            left.ArgumentIsNotNull(nameof(left));
            right.ArgumentIsNotNull(nameof(right));

            if (!left.Type.IsPrimitive || !right.Type.IsPrimitive)
            {
                var mn = @operator.GetMethodName();
                if (mn != null)
                {
                    var ta = new[] { left.Type, right.Type };

                    var m = left.Type.GetMethod(mn, BindingFlags.Static | BindingFlags.Public, null, ta, null)
                            ?? (right.Type != left.Type ? right.Type.GetMethod(mn, BindingFlags.Static | BindingFlags.Public, null, ta, null) : null);

                    if (m != null)
                    {
                        return new BinaryExpression(left, right, @operator, m);
                    }
                }
            }

            return new BinaryExpression(left, right, @operator);
        }

        public static BinaryExpression MakeBinary(this Expression left, Expression right, BinaryOperator @operator, MethodInfo method)
        {
            left.ArgumentIsNotNull(nameof(left));
            right.ArgumentIsNotNull(nameof(right));

            if (method != null)
            {
                if (!method.IsPublic
                    || !method.IsStatic
                    || method.Name != @operator.GetMethodName()
                    || (method.DeclaringType != left.Type && method.DeclaringType != right.Type))
                {
                    throw new ArgumentException();
                }
                return new BinaryExpression(left, right, @operator, method);
            }

            return new BinaryExpression(left, right, @operator);
        }

        public static BinaryExpression Add(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.Add);

        public static BinaryExpression Add(this Expression left, Expression right, MethodInfo method)
            => left.MakeBinary(right, BinaryOperator.Add, method);

        public static BinaryExpression AddChecked(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.AddChecked);

        public static BinaryExpression Subtract(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.Subtract);

        public static BinaryExpression Subtract(this Expression left, Expression right, MethodInfo method)
            => left.MakeBinary(right, BinaryOperator.Subtract, method);

        public static BinaryExpression SubtractChecked(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.SubtractChecked);

        public static BinaryExpression Multiply(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.Multiply);

        public static BinaryExpression Multiply(this Expression left, Expression right, MethodInfo method)
            => left.MakeBinary(right, BinaryOperator.Multiply, method);

        public static BinaryExpression MultiplyChecked(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.MultiplyChecked);

        public static BinaryExpression Divide(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.Divide);

        public static BinaryExpression Divide(this Expression left, Expression right, MethodInfo method)
            => left.MakeBinary(right, BinaryOperator.Divide, method);

        public static BinaryExpression Modulo(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.Modulo);

        public static BinaryExpression Modulo(this Expression left, Expression right, MethodInfo method)
            => left.MakeBinary(right, BinaryOperator.Modulo, method);

        public static BinaryExpression And(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.And);

        public static BinaryExpression And(this Expression left, Expression right, MethodInfo method)
            => left.MakeBinary(right, BinaryOperator.And, method);

        public static BinaryExpression Or(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.Or);

        public static BinaryExpression Or(this Expression left, Expression right, MethodInfo method)
            => left.MakeBinary(right, BinaryOperator.Or, method);

        public static BinaryExpression ExclusiveOr(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.ExclusiveOr);

        public static BinaryExpression ExclusiveOr(this Expression left, Expression right, MethodInfo method)
            => left.MakeBinary(right, BinaryOperator.ExclusiveOr, method);

        public static BinaryExpression LeftShift(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.LeftShift);

        public static BinaryExpression LeftShift(this Expression left, Expression right, MethodInfo method)
            => left.MakeBinary(right, BinaryOperator.LeftShift, method);

        public static BinaryExpression RightShift(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.RightShift);

        public static BinaryExpression RightShift(this Expression left, Expression right, MethodInfo method)
            => left.MakeBinary(right, BinaryOperator.RightShift, method);

        public static BinaryExpression AndAlso(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.AndAlso);

        public static BinaryExpression OrElse(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.OrElse);

        public static BinaryExpression Equal(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.Equal);

        public static BinaryExpression Equal(this Expression left, Expression right, MethodInfo method)
            => left.MakeBinary(right, BinaryOperator.Equal, method);

        public static BinaryExpression NotEqual(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.NotEqual);

        public static BinaryExpression NotEqual(this Expression left, Expression right, MethodInfo method)
            => left.MakeBinary(right, BinaryOperator.NotEqual, method);

        public static BinaryExpression GreaterThan(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.GreaterThan);

        public static BinaryExpression GreaterThan(this Expression left, Expression right, MethodInfo method)
            => left.MakeBinary(right, BinaryOperator.GreaterThan, method);

        public static BinaryExpression GreaterThanOrEqual(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.GreaterThanOrEqual);

        public static BinaryExpression GreaterThanOrEqual(this Expression left, Expression right, MethodInfo method)
            => left.MakeBinary(right, BinaryOperator.GreaterThanOrEqual, method);

        public static BinaryExpression LessThan(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.LessThan);

        public static BinaryExpression LessThan(this Expression left, Expression right, MethodInfo method)
            => left.MakeBinary(right, BinaryOperator.LessThan, method);

        public static BinaryExpression LessThanOrEqual(this Expression left, Expression right)
            => left.MakeBinary(right, BinaryOperator.LessThanOrEqual);

        public static BinaryExpression LessThanOrEqual(this Expression left, Expression right, MethodInfo method)
            => left.MakeBinary(right, BinaryOperator.LessThanOrEqual, method);

        #endregion BinaryExpression

        #region AssignmentExpression

        public static AssignmentExpression Assign(this Expression left, Expression right, BinaryOperator @operator = BinaryOperator.Default)
            => new AssignmentExpression(left, right, @operator);

        public static AssignmentExpression AddAssign(this Expression left, Expression right)
            => left.Assign(right, BinaryOperator.Add);

        public static AssignmentExpression AddAssignChecked(this Expression left, Expression right)
            => left.Assign(right, BinaryOperator.AddChecked);

        public static AssignmentExpression SubtractAssign(this Expression left, Expression right)
            => left.Assign(right, BinaryOperator.Subtract);

        public static AssignmentExpression SubtractAssignChecked(this Expression left, Expression right)
            => left.Assign(right, BinaryOperator.SubtractChecked);

        public static AssignmentExpression MultiplyAssign(this Expression left, Expression right)
            => left.Assign(right, BinaryOperator.Multiply);

        public static AssignmentExpression MultiplyAssignChecked(this Expression left, Expression right)
            => left.Assign(right, BinaryOperator.MultiplyChecked);

        public static AssignmentExpression DivideAssign(this Expression left, Expression right)
            => left.Assign(right, BinaryOperator.Divide);

        #endregion AssignmentExpression

        #region Property

        public static MemberExpression MakeMemberAccess(this Expression @object, MemberInfo member)
            => new MemberExpression(@object, member);

        public static MemberExpression Property(this Expression @object, PropertyInfo property)
            => new MemberExpression(@object, property);

        // TODO:   public static MemberExpression Property(this Expression @object, string propertyName)=>@object.Property(@object.Type.GetProperty(propertyName));

        public static MemberExpression Event(this Expression @object, EventInfo property)
            => new MemberExpression(@object, property);

        // TODO:   public static MemberExpression Event(this Expression @object, string propertyName)=>@object.Property(@object.Type.GetEvent(propertyName));

        public static IndexExpression MakeIndex(this Expression @object, params Expression[] parameters)
            => new IndexExpression(@object, parameters);

        public static IndexExpression MakeIndex(this Expression @object, IEnumerable<Expression> parameters)
            => new IndexExpression(@object, parameters);

        public static IndexExpression MakeIndex(this Expression @object, PropertyInfo indexer, params Expression[] parameters)
            => new IndexExpression(@object, indexer, parameters);

        public static IndexExpression MakeIndex(this Expression @object, PropertyInfo indexer, IEnumerable<Expression> parameters)
            => new IndexExpression(@object, indexer, parameters);

        #endregion Property

        public static ReturnStatement ToReturnStatement(this Expression value)
            => new ReturnStatement(value);

        public static ExpressionStatement ToStatement(this Expression value)
            => new ExpressionStatement(value);
    }
}