using System;
using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class UnaryExpression : Expression
    {
        public UnaryExpression(Expression operand, UnaryOperator @operator)
        {
            if (@operator.IsConvert())
            {
                throw new ArgumentException($"Unsupported {nameof(@operator)}");
            }

            operand.ArgumentIsNotNull(nameof(operand));

            Operand = operand;
            Operator = @operator;
        }

        public UnaryExpression(Expression operand, UnaryOperator @operator, Type type)
        {
            if (!@operator.IsConvert())
            {
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
                case UnaryOperator.UnaryPlus:
                    writer.Write('+');
                    break;

                case UnaryOperator.UnaryNegation:
                    writer.Write('-');
                    break;

                case UnaryOperator.LogicalNot:
                    writer.Write('!');
                    break;

                case UnaryOperator.OnesComplement:
                    writer.Write('~');
                    break;

                case UnaryOperator.PreIncrement:
                    writer.Write("++");
                    break;

                case UnaryOperator.PreDecrement:
                    writer.Write("--");
                    break;

                case UnaryOperator.PostIncrement:
                case UnaryOperator.PostDecrement:
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

            if (Operator == UnaryOperator.PostIncrement)
            {
                writer.Write("++");
            }
            else if (Operator == UnaryOperator.PostDecrement)
            {
                writer.Write("--");
            }
        }

        internal override Expression ReduceCore()
        {
            switch (Operator)
            {
                case UnaryOperator.UnaryPlus:
                    return Operand;

                case UnaryOperator.UnaryNegation:
                case UnaryOperator.LogicalNot:
                case UnaryOperator.OnesComplement:
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
            => @operator.IsConvert() ? new UnaryExpression(operand, @operator, type) : new UnaryExpression(operand, @operator);
    }
}