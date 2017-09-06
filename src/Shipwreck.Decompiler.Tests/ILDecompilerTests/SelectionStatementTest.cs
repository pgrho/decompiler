using Xunit;
using Xunit.Abstractions;

namespace Shipwreck.Decompiler.ILDecompilerTests
{
    public sealed class SelectionStatementTest : ILDecompilerTestBase
    {
        public SelectionStatementTest(ITestOutputHelper output = null)
            : base(output)
        {
        }

        #region Branch

        private static int BranchTrue(bool c, int a)
        {
            var v = a;
            if (c)
            {
                v += 20;
            }
            else
            {
                v -= 20;
            }
            v *= 3;
            return v;
        }

        private static int BranchFalse(bool c, int a)
        {
            var v = a;
            if (c)
            {
                v += 20;
            }

            return v;
        }

        #endregion Branch

        #region Switch

        private static int Switch(int a)
        {
            switch (a)
            {
                case 1:
                    return 1;

                case 3:
                    return 333;

                case 5:
                    return 55555;

                case 7:
                    return 7777777;
            }
            return -1;
        }

        [Theory]
        [InlineData(nameof(BranchTrue))]
        [InlineData(nameof(BranchFalse))]
        [InlineData(nameof(Switch))]
        public void NotImplementedTest(string n)
            => AssertMethod(GetMethod(n));

        #endregion Switch
    }
}