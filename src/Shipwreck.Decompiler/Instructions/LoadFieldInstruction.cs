using System.Reflection;
using Shipwreck.CSharpModels.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadFieldInstruction : UnaryExpressionInstruction
    {
        public LoadFieldInstruction(FieldInfo field)
        {
            field.ArgumentIsNotNull(nameof(field));

            Field = field;
        }

        public FieldInfo Field { get; }

        internal override Expression CreateExpression(DecompilationContext context, Expression value)
            => value.MakeMemberAccess(Field);

        public override bool IsEqualTo(Instruction other)
            => this == other
            || (other is LoadFieldInstruction ui && Field == ui.Field);

        public override string ToString()
            => "ldfld " + Field;
    }
}