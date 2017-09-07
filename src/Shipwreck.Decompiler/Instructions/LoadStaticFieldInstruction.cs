using System.Reflection;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadStaticFieldInstruction : LoadConstantInstruction
    {
        public LoadStaticFieldInstruction(FieldInfo field)
        {
            field.ArgumentIsNotNull(nameof(field));
            Field = field;
        }

        public FieldInfo Field { get; }

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = new MemberExpression(Field);
            return true;
        }

        public override bool IsEqualTo(Instruction other)
            => this == (object)other
            && (other is LoadStaticFieldInstruction li && Field == li.Field);

        public override string ToString()
            => $"ldsfld " + Field;
    }
}