using System.Reflection;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadStaticFieldAddressInstruction : LoadConstantInstruction
    {
        public LoadStaticFieldAddressInstruction(FieldInfo field)
        {
            field.ArgumentIsNotNull(nameof(field));
            Field = field;
        }

        public FieldInfo Field { get; }

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            expression = new MemberExpression(Field).AddressOf();
            return true;
        }

        public override bool IsEqualTo(Syntax other)
            => this == (object)other
            && (other is LoadStaticFieldAddressInstruction li && Field == li.Field);

        public override string ToString()
            => $"ldsflda " + Field;
    }
}