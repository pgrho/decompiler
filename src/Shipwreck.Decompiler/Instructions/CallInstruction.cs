using System.Reflection;
using Shipwreck.CSharpModels.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class CallInstruction : CallInstructionBase
    {
        internal CallInstruction(MethodBase method, bool isVirtual)
            : base(method)
        {
            IsVirtual = isVirtual;
        }

        public bool IsVirtual { get; }

        protected override bool HasThis
            => !Method.IsStatic;

        public override bool IsEqualTo(Instruction other)
            => this == (object)other
            || (other is CallInstruction ci && Method == ci.Method && IsVirtual == ci.IsVirtual);

        public override string ToString()
            => (IsVirtual ? "callvirt " : "call ") + Method;

        internal override Expression CreateExpressionCore(Expression obj, Expression[] parameters)
        {
            if (obj is UnaryExpression ue && ue.Operator == UnaryOperator.AddressOf)
            {
                obj = ue.Operand;
            }

            if (Method is ConstructorInfo ci)
            {
                // TODO: constructor call
                return new MethodCallExpression(null, Method, parameters);
            }

            return new MethodCallExpression(IsVirtual && obj is ThisExpression ? new BaseExpression(Method.DeclaringType) : obj, Method, parameters);
        }
    }
}