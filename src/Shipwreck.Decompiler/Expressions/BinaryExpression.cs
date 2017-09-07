using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed partial class BinaryExpression : Expression
    {
        internal BinaryExpression(Expression left, Expression right, BinaryOperator @operator, MethodInfo method = null)
        {
            left.ArgumentIsNotNull(nameof(left));
            right.ArgumentIsNotNull(nameof(right));

            if (@operator == BinaryOperator.Default)
            {
                throw new ArgumentException($"Unsupported {nameof(@operator)}");
            }

            Left = left;
            Right = right;
            Operator = @operator;
            Method = method;
        }

        #region Evaluators

        private static BinaryEvaluator _AddEvaluator;

        private static BinaryEvaluator AddEvaluator
            => _AddEvaluator ?? (_AddEvaluator = new BinaryEvaluator(System.Linq.Expressions.ExpressionType.Add));

        private static BinaryEvaluator _SubtractEvaluator;

        private static BinaryEvaluator SubtractEvaluator
            => _SubtractEvaluator ?? (_SubtractEvaluator = new BinaryEvaluator(System.Linq.Expressions.ExpressionType.Subtract));

        private static BinaryEvaluator _MultiplyEvaluator;

        private static BinaryEvaluator MultiplyEvaluator
            => _MultiplyEvaluator ?? (_MultiplyEvaluator = new BinaryEvaluator(System.Linq.Expressions.ExpressionType.Multiply));

        private static BinaryEvaluator _DivideEvaluator;

        private static BinaryEvaluator DivideEvaluator
            => _DivideEvaluator ?? (_DivideEvaluator = new BinaryEvaluator(System.Linq.Expressions.ExpressionType.Divide));

        private static BinaryEvaluator _ModuloEvaluator;

        private static BinaryEvaluator ModuloEvaluator
            => _ModuloEvaluator ?? (_ModuloEvaluator = new BinaryEvaluator(System.Linq.Expressions.ExpressionType.Modulo));

        #endregion Evaluators

        public Expression Left { get; }
        public Expression Right { get; }
        public BinaryOperator Operator { get; }

        public MethodInfo Method { get; }

        private Type _Type;

        public override Type Type
            => Method?.ReturnType ?? _Type ?? (_Type = GetResultType());

        private Type GetResultType()
        {
            switch (Operator)
            {
                case BinaryOperator.Equal:
                case BinaryOperator.NotEqual:
                case BinaryOperator.GreaterThan:
                case BinaryOperator.GreaterThanOrEqual:
                case BinaryOperator.LessThan:
                case BinaryOperator.LessThanOrEqual:
                case BinaryOperator.AndAlso:
                case BinaryOperator.OrElse:
                    return typeof(bool);

                case BinaryOperator.LeftShift:
                case BinaryOperator.RightShift:
                    return Left.Type;

                case BinaryOperator.NullCoalesce:
                    return Left.Type.IsAssignableFrom(Right.Type) ? Left.Type : Right.Type;
            }

            var lt = Left.Type;
            var rt = Right.Type;

            if (lt == typeof(double) || lt == typeof(double))
            {
                return typeof(double);
            }
            if (lt == typeof(float) || lt == typeof(float))
            {
                return typeof(float);
            }
            if (lt == typeof(IntPtr) || lt == typeof(IntPtr))
            {
                return typeof(IntPtr);
            }
            if (lt == typeof(UIntPtr) || lt == typeof(UIntPtr))
            {
                return typeof(UIntPtr);
            }
            if (rt.IsPointer)
            {
                return lt;
            }
            if (rt.IsPointer)
            {
                return lt;
            }
            if (lt == typeof(ulong) || lt == typeof(ulong))
            {
                return typeof(ulong);
            }
            if (lt == typeof(long) || lt == typeof(long))
            {
                return typeof(long);
            }
            if (lt == typeof(uint) || lt == typeof(uint))
            {
                return typeof(uint);
            }
            return typeof(int);
        }

        public override bool IsEqualTo(Syntax other)
            => this == (object)other
                || (other is BinaryExpression be
                    && Left.IsEqualTo(be.Left)
                    && Right.IsEqualTo(be.Right)
                    && Operator == be.Operator);

        internal override Expression ReduceCore()
        {
            if (Left.TryReduce(out var l) | Right.TryReduce(out var r))
            {
                return new BinaryExpression(l, r, Operator);
            }

            if (Left is ConstantExpression lce
                && Right is ConstantExpression rce
                && lce.Type.IsPrimitive
                && rce.Type.IsPrimitive)
            {
                switch (Operator)
                {
                    case BinaryOperator.Add:
                        return AddEvaluator.Evaluate(lce.Value, rce.Value).ToExpression();

                    case BinaryOperator.Subtract:
                        return SubtractEvaluator.Evaluate(lce.Value, rce.Value).ToExpression();

                    case BinaryOperator.Multiply:
                        return MultiplyEvaluator.Evaluate(lce.Value, rce.Value).ToExpression();

                    case BinaryOperator.Divide:
                        return DivideEvaluator.Evaluate(lce.Value, rce.Value).ToExpression();

                    case BinaryOperator.Modulo:
                        return ModuloEvaluator.Evaluate(lce.Value, rce.Value).ToExpression();
                }
            }

            switch (Operator)
            {
                case BinaryOperator.LeftShift:
                case BinaryOperator.RightShift:
                    if (Left.Type.IsPrimitive
                        && Right is BinaryExpression be
                        && be.Operator == BinaryOperator.And
                        && be.Right is ConstantExpression ace
                        && ace.Value is int mask
                        && Math.Min(4, Marshal.SizeOf(Left.Type)) * 8 == mask + 1)
                    {
                        return Left.MakeBinary(be.Left, Operator);
                    }
                    break;
            }

            return base.ReduceCore();
        }

        internal override Expression ReplaceCore(Expression currentExpression, Expression newExpression, bool replaceAll, bool allowConditional)
        {
            if (IsEqualTo(currentExpression))
            {
                return newExpression;
            }

            var l = Left.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional);

            if (!allowConditional && l == Left)
            {
                // TODO: LogicalOperator
            }

            var r = replaceAll || l == Left ? Right.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional) : Right;

            return l == Left && r == Right ? this : new BinaryExpression(l, r, Operator);
        }

        public override ExpressionPrecedence Precedence
        {
            get
            {
                switch (Operator)
                {
                    case BinaryOperator.AddChecked:
                    case BinaryOperator.SubtractChecked:
                    case BinaryOperator.MultiplyChecked:
                        return ExpressionPrecedence.Primary;

                    case BinaryOperator.Multiply:
                    case BinaryOperator.Divide:
                    case BinaryOperator.Modulo:
                        return ExpressionPrecedence.Multiplicative;

                    case BinaryOperator.Add:
                    case BinaryOperator.Subtract:
                        return ExpressionPrecedence.Additive;

                    case BinaryOperator.LeftShift:
                    case BinaryOperator.RightShift:
                        return ExpressionPrecedence.Shift;

                    case BinaryOperator.GreaterThan:
                    case BinaryOperator.GreaterThanOrEqual:
                    case BinaryOperator.LessThan:
                    case BinaryOperator.LessThanOrEqual:
                        // TODO:is
                        return ExpressionPrecedence.Relational;

                    case BinaryOperator.Equal:
                    case BinaryOperator.NotEqual:
                        return ExpressionPrecedence.Equality;

                    case BinaryOperator.And:
                        return ExpressionPrecedence.And;

                    case BinaryOperator.ExclusiveOr:
                        return ExpressionPrecedence.ExclusiveOr;

                    case BinaryOperator.Or:
                        return ExpressionPrecedence.Or;

                    case BinaryOperator.AndAlso:
                        return ExpressionPrecedence.AndAlso;

                    case BinaryOperator.OrElse:
                        return ExpressionPrecedence.OrElse;

                    case BinaryOperator.NullCoalesce:
                        return ExpressionPrecedence.NullCoalescing;
                }

                return ExpressionPrecedence.Primary;
            }
        }

        public override IEnumerable<Expression> GetChildren()
        {
            yield return Left;
            yield return Right;
        }
    }
}