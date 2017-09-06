using Shipwreck.Decompiler.Expressions;
using Xunit;
using Xunit.Abstractions;

namespace Shipwreck.Decompiler.ILDecompilerTests
{
    public sealed class ArgumentTest : ILDecompilerTestBase
    {
        public ArgumentTest(ITestOutputHelper output = null)
            : base(output)
        {
        }

        private static int LoadArgument_0(int a0) => a0;

        private static int LoadArgument_1(int a0, int a1) => a1;

        private static int LoadArgument_2(int a0, int a1, int a2) => a2;

        private static int LoadArgument_3(int a0, int a1, int a2, int a3) => a3;

        private int LoadArgument_Instance_0(int a0) => a0;

        private int LoadArgument_Instance_1(int a0, int a1) => a1;

        private int LoadArgument_Instance_2(int a0, int a1, int a2) => a2;

        private int LoadArgument_Instance_3(int a0, int a1, int a2, int a3) => a3;

        [Theory]
        [InlineData(nameof(LoadArgument_0), 0)]
        [InlineData(nameof(LoadArgument_1), 1)]
        [InlineData(nameof(LoadArgument_2), 2)]
        [InlineData(nameof(LoadArgument_3), 3)]
        [InlineData(nameof(LoadArgument_Instance_0), 0)]
        [InlineData(nameof(LoadArgument_Instance_1), 1)]
        [InlineData(nameof(LoadArgument_Instance_2), 2)]
        [InlineData(nameof(LoadArgument_Instance_3), 3)]
        // TODO: test ldarg.s
        // TODO: test ldarg
        // TODO: test ldarga.s
        // TODO: test ldarga
        public void LoadArgumentTest(string m, int i)
            => AssertMethod(GetMethod(m), new ParameterExpression("a" + i, typeof(int)).ToReturnStatement());

        private ArgumentTest LoadThis() => this;

        [Fact]
        public void LoadArgumentTest_This()
            => AssertMethod(GetMethod(nameof(LoadThis)), new ThisExpression(GetType()).ToReturnStatement());
    }
}