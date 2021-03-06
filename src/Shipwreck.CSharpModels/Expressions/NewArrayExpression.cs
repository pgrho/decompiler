using System;
using System.Collections.Generic;

namespace Shipwreck.CSharpModels.Expressions
{
    public sealed partial class NewArrayExpression : Expression
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

        public override bool IsEqualTo(Syntax other)
            => this == other
            || (other is NewArrayExpression de
                && Type.IsEqualTo(de.Type)
                && Length.IsEqualTo(Length));

        public override ExpressionPrecedence Precedence => ExpressionPrecedence.Primary;

        internal override Expression ReduceCore()
        {
            var l = Length.Reduce();
            return l == Length ? this : new NewArrayExpression(Type, l);
        }

        internal override Expression ReplaceCore(Expression currentExpression, Expression newExpression, bool replaceAll, bool allowConditional)
        {
            if (IsEqualTo(currentExpression))
            {
                return newExpression;
            }

            var l = Length.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional);

            return l == Length ? this : new NewArrayExpression(Type, l);
        }

        public override IEnumerable<Expression> GetChildren()
        {
            if (Length != null)
            {
                yield return Length;
            }
        }
    }
}