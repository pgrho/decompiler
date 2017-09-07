using System;
using System.Reflection;
using System.Text;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class UnaryInstruction : UnaryExpressionInstruction
    {
        public UnaryInstruction(UnaryOperator @operator)
        {
            Operator = @operator;
        }

        public UnaryOperator Operator { get; }

        internal override Expression CreateExpression(DecompilationContext context, Expression value)
            => value.MakeUnary(Operator);

        public override bool IsEqualTo(Syntax other)
            => this == other
            || (other is UnaryInstruction ui && Operator == ui.Operator);

        public override string ToString()
        {
            var sb = new StringBuilder();

            switch (Operator)
            {
                case UnaryOperator.UnaryNegation:
                    sb.Append("neg");
                    break;

                case UnaryOperator.OnesComplement:
                    sb.Append("not");
                    break;

                default:
                    throw new NotImplementedException();
            }

            return sb.ToString();
        }
    }
}