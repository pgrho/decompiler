using System;
using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class UnaryExpression : Expression
    {
        private Type _Type;

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
            _Type = type;
        }

        #region Evaluators

        private static UnaryEvaluator _OnesComplementEvaluator;
        private static UnaryEvaluator _NegationEvaluator;

        private static UnaryEvaluator OnesComplementEvaluator
            => _OnesComplementEvaluator ?? (_OnesComplementEvaluator = new UnaryEvaluator(System.Linq.Expressions.ExpressionType.OnesComplement));

        private static UnaryEvaluator NegationEvaluator
            => _NegationEvaluator ?? (_NegationEvaluator = new UnaryEvaluator(System.Linq.Expressions.ExpressionType.Negate));

        #endregion Evaluators

        public Expression Operand { get; }

        public UnaryOperator Operator { get; }

        // TODO: Add Method
        public override Type Type
        {
            get
            {
                switch (Operator)
                {
                    case UnaryOperator.Convert:
                    case UnaryOperator.ConvertChecked:
                        return _Type;

                    case UnaryOperator.LogicalNot:
                        return typeof(bool);

                    case UnaryOperator.AddressOf:
                        return Operand.Type.MakePointerType();
                }
                return Operand.Type;
            }
        }

        public override bool IsEquivalentTo(Syntax other)
            => other is UnaryExpression ue
                && Operand.IsEquivalentTo(ue.Operand)
                && Operator == ue.Operator
                && _Type == ue._Type;

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

                case UnaryOperator.AddressOf:
                    writer.Write('&');
                    break;

                case UnaryOperator.PostIncrement:
                case UnaryOperator.PostDecrement:
                    break;

                case UnaryOperator.Convert:
                case UnaryOperator.ConvertChecked:
                    writer.Write('(');
                    writer.Write(_Type.FullName);
                    writer.Write(')');
                    break;

                default:
                    throw new NotImplementedException();
            }

            writer.WriteFirstChild(Operand, this);

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
                case UnaryOperator.Convert:
                case UnaryOperator.ConvertChecked:
                    if (_Type.IsAssignableFrom(Operand.Type))
                    {
                        return Operand;
                    }
                    break;

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
                return Create(l, Operator, _Type);
            }

            if (Operand is ConstantExpression ce)
            {
                switch (Operator)
                {
                    case UnaryOperator.Convert:
                    case UnaryOperator.ConvertChecked:
                        if (ce.Type.IsPrimitive
                            && _Type.IsPrimitive)
                        {
                            return new ConstantExpression(((IConvertible)ce.Value).ToType(_Type, null), _Type);
                        }
                        break;

                    case UnaryOperator.LogicalNot:
                        if (ce.Type.IsPrimitive)
                        {
                            var v = ce.Value is bool b ? b
                                    : ce.Value == null ? false
                                    : ((IConvertible)ce.Value).ToDouble(null) != 0;

                            return (!v).ToExpression();
                        }
                        break;

                    case UnaryOperator.OnesComplement:
                        if (ce.Type.IsPrimitive)
                        {
                            return OnesComplementEvaluator.Evaluate(ce.Value).ToExpression();
                        }
                        break;

                    case UnaryOperator.UnaryNegation:
                        if (ce.Type.IsPrimitive)
                        {
                            return NegationEvaluator.Evaluate(ce.Value).ToExpression();
                        }
                        break;
                }
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

            return op == Operand ? this : Create(op, Operator, _Type);
        }

        private static UnaryExpression Create(Expression operand, UnaryOperator @operator, Type type)
            => @operator.IsConvert() ? new UnaryExpression(operand, @operator, type) : new UnaryExpression(operand, @operator);

        public override ExpressionPrecedence Precedence
        {
            get
            {
                switch (Operator)
                {
                    case UnaryOperator.PostIncrement:
                    case UnaryOperator.PostDecrement:
                        return ExpressionPrecedence.Primary;
                }
                return ExpressionPrecedence.Unary;
            }
        }
    }
}