using System;
using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class UnaryExpression : Expression
    {
        public UnaryExpression(Expression operand, UnaryOperator @operator)
        {
            switch (@operator)
            {
                case UnaryOperator.Negate:
                case UnaryOperator.Not:
                    break;

                default:
                    throw new ArgumentException($"Unsupported {nameof(@operator)}");
            }

            operand.ArgumentIsNotNull(nameof(operand));

            Operand = operand;
            Operator = @operator;
        }

        public UnaryExpression(Expression operand, UnaryOperator @operator, Type type)
        {
            switch (@operator)
            {
                case UnaryOperator.Convert:
                case UnaryOperator.ConvertChecked:
                    break;

                default:
                    throw new ArgumentException($"Unsupported {nameof(@operator)}");
            }

            operand.ArgumentIsNotNull(nameof(operand));
            type.ArgumentIsNotNull(nameof(type));

            Operand = operand;
            Operator = @operator;
            Type = type;
        }

        public Expression Operand { get; }

        public UnaryOperator Operator { get; }

        // TODO: Add Expression.Type
        internal Type Type { get; }

        public override bool IsEquivalentTo(Syntax other)
            => other is UnaryExpression ue
                && Operand.IsEquivalentTo(ue.Operand)
                && Operator == ue.Operator
                && Type == ue.Type;

        public override void WriteTo(TextWriter writer)
        {
            switch (Operator)
            {
                case UnaryOperator.Not:
                    writer.Write('~');
                    break;

                case UnaryOperator.Negate:
                    writer.Write('!'); // TODO: minus sign for non-bool operand
                    break;

                case UnaryOperator.Convert:
                case UnaryOperator.ConvertChecked:
                    writer.Write('(');
                    writer.Write(Type.FullName);
                    writer.Write(')');
                    break;

                default:
                    throw new NotImplementedException();
            }

            writer.Write('(');
            Operand.WriteTo(writer);
            writer.Write(')');
        }

        internal override Expression ReduceCore()
        {
            switch (Operator)
            {
                case UnaryOperator.Negate:
                case UnaryOperator.Not:
                    if (Operand is UnaryExpression u && u.Operator == Operator)
                    {
                        return u.Operand;
                    }
                    break;
            }

            if (Operand.TryReduce(out var l))
            {
                return Create(l, Operator, Type);
            }

            return base.ReduceCore();
        }

        internal override Expression ReplaceCore(Expression currentExpression, Expression newExpression, bool replaceAll, bool allowConditional)
        {
            if (IsEquivalentTo(currentExpression))
            {
                return newExpression;
            }

            var op = Operand.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional);

            return op == Operand ? this : Create(op, Operator, Type);
        }

        private static UnaryExpression Create(Expression operand, UnaryOperator @operator, Type type)
        {
            switch (@operator)
            {
                case UnaryOperator.Negate:
                case UnaryOperator.Not:
                    return new UnaryExpression(operand, @operator);
            }
            return new UnaryExpression(operand, @operator, type);
        }
    }
}