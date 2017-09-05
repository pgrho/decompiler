using System;
using System.IO;
using System.Reflection;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class BinaryExpression : Expression
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

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
                || (other is BinaryExpression be
                    && Left.IsEquivalentTo(be.Left)
                    && Right.IsEquivalentTo(be.Right)
                    && Operator == be.Operator);

        public override void WriteTo(TextWriter writer)
        {
            var isChecked = Operator.IsChecked();

            if (isChecked)
            {
                writer.Write("checked(");
            }

            writer.WriteFirstChild(Left, this);

            writer.Write(' ');
            writer.Write(Operator.GetToken());
            writer.Write(' ');

            writer.WriteSecondChild(Right, this);

            if (isChecked)
            {
                writer.Write(')');
            }
        }

        internal override Expression ReduceCore()
        {
            if (Left.TryReduce(out var l) | Right.TryReduce(out var r))
            {
                return new BinaryExpression(l, r, Operator);
            }

            return base.ReduceCore();
        }

        internal override Expression ReplaceCore(Expression currentExpression, Expression newExpression, bool replaceAll, bool allowConditional)
        {
            if (IsEquivalentTo(currentExpression))
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
                }

                return ExpressionPrecedence.Primary;
            }
        }
    }
}