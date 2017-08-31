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

        #region LoadNullTest

        private static object LoadNull() => null;

        [Theory]
        [InlineData(nameof(LoadNull))]
        public void LoadNullTest(string methodName)
        {
            var ret = ILDecompiler.Decompile(GetMethod(methodName));

            Assert.Equal(1, ret.Count);
            Assert.True(((object)null).ToExpression().ToReturnStatement().IsEquivalentTo(ret[0]));
        }

        #endregion LoadNullTest

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

        #endregion LoadInt32Test

        #region LoadInt64Test

        private static long LoadInt64() => 0x123456789abcdef0;

        [Theory]
        [InlineData(nameof(LoadInt64), 0x123456789abcdef0)]
        public void LoadInt64Test(string methodName, long value)
        {
            var ret = ILDecompiler.Decompile(GetMethod(methodName));

            Assert.Equal(1, ret.Count);
            Assert.True(value.ToExpression().ToReturnStatement().IsEquivalentTo(ret[0]));
        }

        #endregion LoadInt64Test

        #region LoadSingleTest

        private static float LoadSingle() => float.MaxValue;

        [Theory]
        [InlineData(nameof(LoadSingle), float.MaxValue)]
        public void LoadSingleTest(string methodName, float value)
        {
            var ret = ILDecompiler.Decompile(GetMethod(methodName));

            Assert.Equal(1, ret.Count);
            Assert.True(value.ToExpression().ToReturnStatement().IsEquivalentTo(ret[0]));
        }

        #endregion LoadSingleTest

        #region LoadDoubleTest

        private static double LoadDouble() => float.MaxValue * float.MaxValue;

        [Theory]
        [InlineData(nameof(LoadDouble), float.MaxValue * float.MaxValue)]
        public void LoadDoubleTest(string methodName, double value)
        {
            var ret = ILDecompiler.Decompile(GetMethod(methodName));

            Assert.Equal(1, ret.Count);
            Assert.True(value.ToExpression().ToReturnStatement().IsEquivalentTo(ret[0]));
        }

        #endregion LoadDoubleTest

        #endregion Constant

        //#region Unary

        //#region NotTest

        //private static int Not(int a) => ~a;

        //[Theory]
        //[InlineData(nameof(Not))]
        //public void NotTest(string methodName)
        //{
        //    var ret = ILDecompiler.Decompile(GetMethod(methodName));

        //    Assert.Equal(1, ret.Count);
        //    Assert.True(8.ToExpression().Not().ToReturnStatement().IsEquivalentTo(ret[0]));
        //}

        //#endregion NotTest

        //#endregion Unary
    }
}