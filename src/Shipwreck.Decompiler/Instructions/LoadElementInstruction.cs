using Shipwreck.CSharpModels.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadElementInstruction : BinaryExpressionInstruction
    {
        internal override Expression CreateExpression(Expression arg1, Expression arg2)
            => arg1.MakeIndex(arg2);

        public override bool IsEqualTo(Instruction other)
            => this == other || other is LoadElementInstruction;

        public override string ToString()
            => "ldelem";
    }
}