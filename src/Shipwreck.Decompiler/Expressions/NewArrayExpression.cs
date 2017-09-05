using System;
using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class NewArrayExpression : Expression
    {
        public NewArrayExpression(Type elementType, Expression length)
        {
            elementType.ArgumentIsNotNull(nameof(elementType));
            length.ArgumentIsNotNull(nameof(length));

            Type = elementType.MakeArrayType();
            Length = length;
        }

        public override Type Type { get; }

        public Expression Length { get; }

        public override bool IsEquivalentTo(Syntax other)
            => this == other
            || (other is NewArrayExpression de
                && Type == de.Type
                && Length.IsEquivalentTo(Length));

        public override ExpressionPrecedence Precedence => ExpressionPrecedence.Primary;

        public override void WriteTo(TextWriter writer)
        {
            writer.Write("new ");
            writer.Write(Type.GetElementType().FullName);
            writer.Write('[');
            Length.WriteTo(writer);
            writer.Write(']');
        }

        internal override Expression ReduceCore()
        {
            var l = Length.Reduce();
            return l == Length ? this : new NewArrayExpression(Type, l);
        }

        internal override Expression ReplaceCore(Expression currentExpression, Expression newExpression, bool replaceAll, bool allowConditional)
        {
            if (IsEquivalentTo(currentExpression))
            {
                return newExpression;
            }

            var l = Length.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional);

            return l == Length ? this : new NewArrayExpression(Type, l);
        }
    }
}