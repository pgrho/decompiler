using System.Reflection;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class NewObjectInstruction : CallInstructionBase
    {
        internal NewObjectInstruction(ConstructorInfo method)
            : base(method)
        {
        }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is NewObjectInstruction noi && Method == noi.Method);

        public override string ToString()
            => "newobj " + Method;

        internal override Expression CreateExpressionCore(Expression obj, Expression[] parameters)
            => new NewExpression((ConstructorInfo)Method, parameters);
    }
}