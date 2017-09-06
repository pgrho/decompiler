using System;
using Xunit;
using Xunit.Abstractions;

namespace Shipwreck.Decompiler.ILDecompilerTests
{
    public sealed class MethodCallExpressionTest : ILDecompilerTestBase
    {
        public MethodCallExpressionTest(ITestOutputHelper output = null)
            : base(output)
        {
        }

        private static IntPtr StaticMethod() => new IntPtr(0);

        private static IntPtr Call() => StaticMethod();

        private string CallVirt() => ToString();

        private string CallBase() => base.ToString();

        [Theory]
        [InlineData(nameof(Call))]
        [InlineData(nameof(CallVirt))]
        [InlineData(nameof(CallBase))]
        public void NotImplementedTest(string methodName)
        {
            AssertMethod(GetMethod(methodName));
        }

        // TODO: constructor call
    }
}