using System;
using System.Linq;
using Shipwreck.Decompiler.Expressions;
using Xunit;
using Xunit.Abstractions;

namespace Shipwreck.Decompiler.ILDecompilerTests
{
    public sealed class PrimaryExpressionTest : ILDecompilerTestBase
    {
        public PrimaryExpressionTest(ITestOutputHelper output = null)
            : base(output)
        {
        }

        #region Load Constant

        #region LoadInt32Test

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

        #endregion LoadInt32Test

        private static object LoadNull() => null;

        private static long LoadInt64() => 0x123456789abcdef0;

        private static float LoadSingle() => float.MaxValue;

        private static double LoadDouble() => float.MaxValue * float.MaxValue;

        private static string LoadString() => "hoge";

        [Theory]
        [InlineData(nameof(LoadInt32_M1))]
        [InlineData(nameof(LoadInt32_0))]
        [InlineData(nameof(LoadInt32_1))]
        [InlineData(nameof(LoadInt32_2))]
        [InlineData(nameof(LoadInt32_3))]
        [InlineData(nameof(LoadInt32_4))]
        [InlineData(nameof(LoadInt32_5))]
        [InlineData(nameof(LoadInt32_6))]
        [InlineData(nameof(LoadInt32_7))]
        [InlineData(nameof(LoadInt32_8))]
        [InlineData(nameof(LoadInt32_127))]
        [InlineData(nameof(LoadInt32_0x12345678))]
        [InlineData(nameof(LoadInt64))]
        [InlineData(nameof(LoadNull))]
        [InlineData(nameof(LoadSingle))]
        [InlineData(nameof(LoadDouble))]
        [InlineData(nameof(LoadString))]
        public void LoadStringTest(string methodName)
        {
            var m = GetMethod(methodName);
            var v = m.Invoke(null, null);

            AssertMethod(m, v.ToExpression().ToReturnStatement());
        }

        #endregion Load Constant

        #region NewObject

        private static IntPtr NewObjectIntPtr() => new IntPtr(1);

        private static DateTime NewObjectDateTime() => new DateTime(1, 2, 3, 4, 5, 6, 7);

        [Theory]
        [InlineData(nameof(NewObjectIntPtr), 1)]
        [InlineData(nameof(NewObjectDateTime), 7)]
        public void NewObjectTest(string methodName, int count)
        {
            var m = GetMethod(methodName);
            var t = m.ReturnType;
            var expected = new NewExpression(t.GetConstructor(Enumerable.Repeat(typeof(int), count).ToArray()), Enumerable.Range(1, count).Select(i => i.ToExpression())).ToReturnStatement();

            AssertMethod(m, expected);
        }

        #endregion NewObject

        private static int[] NewArray() => new int[0];

        [Theory]
        [InlineData(nameof(NewArray))]
        public void NotImplementedTest(string m)
            => AssertMethod(GetMethod(m));
    }
}