using Shipwreck.CSharpModels.Expressions;
using Xunit;
using Xunit.Abstractions;

namespace Shipwreck.Decompiler.ILDecompilerTests
{
    public sealed class UnaryExpressionTest : ILDecompilerTestBase
    {
        public UnaryExpressionTest(ITestOutputHelper output = null)
            : base(output)
        {
        }

        private static int OnesComplement(int a) => ~a;

        private static int Negate(int a) => -a;

        [Theory]
        [InlineData(nameof(OnesComplement), UnaryOperator.OnesComplement)]
        [InlineData(nameof(Negate), UnaryOperator.UnaryNegation)]
        public void OnesComplementTest(string methodName, UnaryOperator op)
            => AssertMethod(GetMethod(methodName), new ParameterExpression("a", typeof(int)).MakeUnary(op).ToReturnStatement());
    }
}