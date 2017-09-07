using System.Reflection;
using System.Reflection.Emit;
using Shipwreck.CSharpModels.Expressions;
using Shipwreck.CSharpModels.Statements;

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

        protected abstract bool HasThis { get; }

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            if (context.GetFromCount(this) <= 1)
            {
                var pc = Method.GetParameters().Length;
                Expression[] ps = null;

                var i = index;

                for (var j = pc - 1; j >= 0; j--)
                {
                    i--;
                    if (context.TryCreateExpression(ref i, out var e))
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
                if (HasThis)
                {
                    i--;

                    if (context.TryCreateExpression(ref i, out var e))
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

        internal override bool TryCreateStatement(DecompilationContext context, ref int startIndex, ref int lastIndex, out Statement statement)
        {
            if (TryCreateExpression(context, ref startIndex, out var e))
            {
                statement = e.ToStatement();
                return true;
            }
            statement = null;
            return false;
        }

        internal abstract Expression CreateExpressionCore(Expression obj, Expression[] parameters);
    }
}