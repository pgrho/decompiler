using System.Reflection;
using Shipwreck.Decompiler.Expressions;

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

        public override bool IsEquivalentTo(Syntax other)
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
            return new MethodCallExpression(IsVirtual && obj is ThisExpression ? new BaseExpression() : obj, (MethodInfo)Method, parameters);
        }
    }
}