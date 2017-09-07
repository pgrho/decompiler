using System.Reflection;
using Shipwreck.CSharpModels.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadFieldAddressInstruction : UnaryExpressionInstruction
    {
        public LoadFieldAddressInstruction(FieldInfo field)
        {
            field.ArgumentIsNotNull(nameof(field));

            Field = field;
        }

        public FieldInfo Field { get; }

        internal override Expression CreateExpression(DecompilationContext context, Expression value)
            => value.MakeMemberAccess(Field);

        public override bool IsEqualTo(Instruction other)
            => this == other
            || (other is LoadFieldAddressInstruction ui && Field == ui.Field);

        public override string ToString()
            => "ldflda " + Field;
    }
}