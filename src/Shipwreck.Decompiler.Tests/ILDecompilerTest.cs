using System.Reflection;
using Shipwreck.Decompiler.Expressions;
using Xunit;

namespace Shipwreck.Decompiler
{
    public class ILDecompilerTest
    {
        private static MethodInfo GetMethod(string name)
            => typeof(ILDecompilerTest).GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

        #region Constant

        private static int LoadInt32_M1() => -1;

        private static int LoadInt32_0() => 0;

        private static int LoadInt32_1() => 1;

        private static int LoadInt32_2() => 2;

        private static int LoadInt32_3() => 3;

        private static int LoadInt32_4() => 4;

        private static int LoadInt32_5() => 5;

        private static int LoadInt32_6() => 6;

        private static int LoadInt32_7() => 7;

        private static int LoadInt32_8() => 8;

        private static int LoadInt32_127() => 127;

        private static int LoadInt32_0x12345678() => 0x12345678;

        [Theory]
        [InlineData(nameof(LoadInt32_M1), -1)]
        [InlineData(nameof(LoadInt32_0), 0)]
        [InlineData(nameof(LoadInt32_1), 1)]
        [InlineData(nameof(LoadInt32_2), 2)]
        [InlineData(nameof(LoadInt32_3), 3)]
        [InlineData(nameof(LoadInt32_4), 4)]
        [InlineData(nameof(LoadInt32_5), 5)]
        [InlineData(nameof(LoadInt32_6), 6)]
        [InlineData(nameof(LoadInt32_7), 7)]
        [InlineData(nameof(LoadInt32_8), 8)]
        [InlineData(nameof(LoadInt32_127), 127)]
        [InlineData(nameof(LoadInt32_0x12345678), 0x12345678)]
        public void LoadInt32Test(string methodName, int value)
        {
            var ret = ILDecompiler.Decompile(GetMethod(methodName));

            Assert.Equal(1, ret.Count);
            Assert.True(value.ToExpression().ToReturnStatement().IsEquivalentTo(ret[0]));
        }

        #endregion Constant
    }
}