using System.Reflection;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class StoreStaticFieldInstruction : UnaryAssignmentInstruction
    {
        public StoreStaticFieldInstruction(FieldInfo field)
        {
            Field = field;
        }

        public FieldInfo Field { get; }

        internal override Expression CreateExpression(DecompilationContext context, Expression value)
                => new MemberExpression(Field).Assign(value);

        public override bool IsEqualTo(Syntax other)
            => this == (object)other
            || (other is StoreStaticFieldInstruction sts && Field == sts.Field);

        public override string ToString()
            => $"stsfld {Field}";
    }
}