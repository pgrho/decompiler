using System.Reflection;
using System.Reflection.Emit;

namespace Shipwreck.Decompiler.Instructions
{
    public abstract class CallInstructionBase : Instruction
    {
        internal CallInstructionBase(MethodBase method)
        {
            method.ArgumentIsNotNull(nameof(method));

            Method = method;
        }

        public MethodBase Method { get; }

        public override FlowControl FlowControl
            => FlowControl.Next;

        public override int PopCount
            => (Method is ConstructorInfo || Method.IsStatic ? 0 : 1) + Method.GetParameters().Length;

        public override int PushCount
            => 1;

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            if (context.GetFromCount(this) <= 1)
            {
                var pc = Method.GetParameters().Length;
                Expression[] ps = null;

                var i = index;

                for (var j = pc - 1; j >= 0; j--)
                {
                    if (--i >= 0
                        && context.RootStatements[i] is Instruction prev
                        && prev != null
                        && prev.TryCreateExpression(context, ref i, out var e))
                    {
                        (ps ?? (ps = new Expression[pc]))[j] = e;
                    }
                    else
                    {
                        expression = null;
                        return false;
                    }
                }

                Expression obj;
                if ((Method as MethodInfo)?.IsStatic == false)
                {
                    if (--i >= 0
                        && context.RootStatements[i] is Instruction prev
                        && prev != null
                        && prev.TryCreateExpression(context, ref i, out var e))
                    {
                        obj = e;
                    }
                    else
                    {
                        expression = null;
                        return false;
                    }
                }
                else
                {
                    obj = null;
                }

                index = i;
                expression = CreateExpressionCore(obj, ps);
                return true;
            }
            expression = null;
            return false;
        }

        internal abstract Expression CreateExpressionCore(Expression obj, Expression[] parameters);

        internal override bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            statement = null;
            return false;
        }
    }
}