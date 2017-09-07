using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class StoreFieldInstruction : Instruction
    {
        public StoreFieldInstruction(FieldInfo field)
        {
            Field = field;
        }

        public FieldInfo Field { get; }

        public override FlowControl FlowControl
            => FlowControl.Next;

        public override int PopCount
            => 2;

        public override int PushCount
            => 0;

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            // If previous instruction is dup

            if (context.GetFromCount(this) <= 1 && index >= 2)
            {
                var j = index - 1;

                if (context.TryCreateExpression(ref j, out var e))
                {
                    if (context.RootStatements[j - 1] is DuplicateInstruction)
                    {
                        j -= 2;
                        if (context.TryCreateExpression(ref j, out var ie))
                        {
                            IEnumerable<MemberBinding> bs = null;
                            var ne = ie as NewExpression;
                            if (ne != null)
                            {
                                bs = new[] { new MemberAssignment(Field, e) };
                            }
                            else if (ie is MemberInitExpression mie)
                            {
                                ne = mie.NewExpression;
                                bs = mie.Bindings.Concat(new[] { new MemberAssignment(Field, e) });
                            }

                            if (ne != null)
                            {
                                index = j;
                                expression = new MemberInitExpression(ne, bs);
                                return true;
                            }
                        }
                    }
                }
            }

            expression = null;
            return false;
        }

        internal override bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            // If previous instruction is not dup
            if (context.GetFromCount(this) <= 1 && startIndex >= 1)
            {
                var j = startIndex - 1;

                if (context.TryCreateExpression(ref j, out var e))
                {
                    j--;
                    if (context.TryCreateExpression(ref j, out var ie))
                    {
                        startIndex = j;
                        statement = CreateStoreExpression(context, ie, e).ToStatement();
                        return true;
                    }
                }
            }

            statement = null;
            return false;
        }

        private Expression CreateStoreExpression(DecompilationContext context, Expression @object, Expression value)
            => @object.MakeMemberAccess(Field).Assign(value);

        public override bool IsEqualTo(Syntax other)
            => this == (object)other
            || (other is StoreFieldInstruction sts && Field == sts.Field);

        public override string ToString()
            => "stfld";
    }
}