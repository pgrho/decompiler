using System;
using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class UnaryExpression : Expression
    {
        public UnaryExpression(Expression operand, UnaryOperator @operator)
        {
            operand.ArgumentIsNotNull(nameof(operand));

            Operand = operand;
            Operator = @operator;
        }

        public Expression Operand { get; }

        public UnaryOperator Operator { get; }

        public override bool IsEquivalentTo(Syntax other)
            => other is UnaryExpression ue
                && Operand.IsEquivalentTo(ue.Operand)
                && Operator == ue.Operator;

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

                default:
                    throw new NotImplementedException();
            }

            writer.Write('(');
            Operand.WriteTo(writer);
            writer.Write(')');
        }
    }
}