using System;
using System.IO;
using System.Reflection;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class PropertyExpression : Expression
    {
        public PropertyExpression(PropertyInfo property)
            : this(null, property)
        {
        }

        public PropertyExpression(Expression @object, PropertyInfo property)
        {
            property.ArgumentIsNotNull(nameof(property));

            var isStatic = (property.GetMethod ?? property.SetMethod).IsStatic;
            if (@object == null)
            {
                if (!isStatic)
                {
                    throw new ArgumentException($"{nameof(@object)} must be specified if the {nameof(property)} is not static.");
                }
            }
            else
            {
                // TODO: if((isStatic || property.DeclaringType.IsAssignableFrom(@object.Type)){ throw; }
            }

            Object = @object;
            Property = property;
        }

        public Expression Object { get; }

        public PropertyInfo Property { get; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is PropertyExpression ne
                && (Object?.IsEquivalentTo(ne.Object) ?? ne.Object == null)
                && Property == ne.Property);

        public override void WriteTo(TextWriter writer)
        {
            if (Object == null)
            {
                writer.Write(Property.DeclaringType.FullName);
            }
            else
            {
                writer.Write('(');
                Object.WriteTo(writer);
                writer.Write(')');
            }
            writer.Write('.');
            writer.Write(Property.Name);
        }

        internal override Expression ReduceCore()
        {
            var o = Object?.ReduceCore();
            return o != Object ? o?.Property(Property) : this;
        }

        internal override Expression ReplaceCore(Expression currentExpression, Expression newExpression, bool replaceAll, bool allowConditional)
        {
            var o = Object?.ReplaceCore(currentExpression, newExpression, replaceAll, allowConditional);
            return o != Object ? o?.Property(Property) : this;
        }
    }
}