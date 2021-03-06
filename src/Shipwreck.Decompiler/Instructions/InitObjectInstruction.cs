using System;
using Shipwreck.CSharpModels.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class InitObjectInstruction : UnaryAssignmentInstruction
    {
        public InitObjectInstruction(Type type)
        {
            type.ArgumentIsNotNull(nameof(type));
            Type = type;
        }

        public Type Type { get; }

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = null;
            return false;
        }

        internal override Expression CreateExpression(DecompilationContext context, Expression value)
        {
            if (value is UnaryExpression ue && ue.Operator == UnaryOperator.AddressOf)
            {
                return ue.Operand.Assign(new DefaultExpression(Type));
            }
            return null;
        }

        public override string ToString()
            => "initobj " + Type.FullName;

        public override bool IsEqualTo(Instruction other)
            => this == (object)other
            && (other is InitObjectInstruction li && Type == li.Type);
    }
}