using Xunit;
using Xunit.Abstractions;

namespace Shipwreck.Decompiler.ILDecompilerTests
{
    public sealed class IterationStatementTest : ILDecompilerTestBase
    {
        public IterationStatementTest(ITestOutputHelper output = null)
            : base(output)
        {
        }

        private static int While(int a)
        {
            var c = a;
            var v = 0;
            while (c > 0)
            {
                v += c;

                c--;
            }
            return v;
        }

        private static void WhileInfinite(int a)
        {
            var c = a;
            var v = 0;
            while (true)
            {
                v += c;

                c--;
            }
        }


        private static int Continue(int c)
        {
            var r = 0;
            for (var i = 0; i < c; i++)
            {
                if (c > 5)
                {
                    continue;
                }
                r *= i;
                if (c > 10)
                {
                    continue;
                }
                r += i;
            }
            return r;
        }

        [Theory]
        [InlineData(nameof(While))]
        [InlineData(nameof(WhileInfinite))]
        [InlineData(nameof(Continue))]
        public void NotImplementedTest(string m)
            => AssertMethod(GetMethod(m));
    }
}