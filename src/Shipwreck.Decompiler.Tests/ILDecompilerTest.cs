using System;
using System.Diagnostics;
using System.Reflection;
using Shipwreck.Decompiler.Expressions;
using Xunit;
using Xunit.Abstractions;

namespace Shipwreck.Decompiler
{
    public class ILDecompilerTest
    {
        private readonly ITestOutputHelper Output;

        public ILDecompilerTest(ITestOutputHelper output = null)
        {
            Output = output;
        }

        [DebuggerStepThrough]
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

        #region Argument

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
        public void LoadArgumentTest(string methodName, int index)
        {
            var ret = ILDecompiler.Decompile(GetMethod(methodName));

            Assert.Equal(1, ret.Count);
            Assert.True(new ParameterExpression("a" + index, typeof(int)).ToReturnStatement().IsEquivalentTo(ret[0]));
        }

        private ILDecompilerTest LoadThis() => this;

        [Fact]
        public void LoadArgumentTest_This()
        {
            var ret = ILDecompiler.Decompile(GetMethod(nameof(LoadThis)));

            Assert.Equal(1, ret.Count);
            Assert.True(new ThisExpression().ToReturnStatement().IsEquivalentTo(ret[0]));
        }

        #endregion Argument

        #region Array Element

        public static sbyte LoadElement_I1(sbyte[] a) => a[1];

        public static byte LoadElement_U1(byte[] a) => a[1];

        public static short LoadElement_I2(short[] a) => a[1];

        public static ushort LoadElement_U2(ushort[] a) => a[1];

        public static int LoadElement_I4(int[] a) => a[1];

        public static uint LoadElement_U4(uint[] a) => a[1];

        public static long LoadElement_I8(long[] a) => a[1];

        public static ulong LoadElement_U8(ulong[] a) => a[1];

        public static IntPtr LoadElement_I(IntPtr[] a) => a[1];

        public static float LoadElement_R4(float[] a) => a[1];

        public static double LoadElement_R8(double[] a) => a[1];

        public static object LoadElement_Ref(object[] a) => a[1];

        public static DateTime LoadElement(DateTime[] a) => a[1];

        [Theory]
        [InlineData(nameof(LoadElement_I1), typeof(sbyte))]
        [InlineData(nameof(LoadElement_U1), typeof(byte))]
        [InlineData(nameof(LoadElement_I2), typeof(short))]
        [InlineData(nameof(LoadElement_U2), typeof(ushort))]
        [InlineData(nameof(LoadElement_I4), typeof(int))]
        [InlineData(nameof(LoadElement_U4), typeof(uint))]
        [InlineData(nameof(LoadElement_I8), typeof(long))]
        [InlineData(nameof(LoadElement_U8), typeof(ulong))]
        [InlineData(nameof(LoadElement_I), typeof(IntPtr))]
        [InlineData(nameof(LoadElement_R4), typeof(float))]
        [InlineData(nameof(LoadElement_R8), typeof(double))]
        [InlineData(nameof(LoadElement_Ref), typeof(object))]
        [InlineData(nameof(LoadElement), typeof(DateTime))]
        public void LoadElementTest(string methodName, Type type)
        {
            var ret = ILDecompiler.Decompile(GetMethod(methodName));

            Assert.Equal(1, ret.Count);
            Assert.True(new ParameterExpression("a", type.MakeArrayType()).ArrayIndex(1.ToExpression()).ToReturnStatement().IsEquivalentTo(ret[0]));
        }

        #endregion Array Element

        #region Store

        #region Local

        public static int StoreLocal(int a)
        {
            var b = a;
            return b + 30;
        }

        public static int StoreLocal_Expression(int a)
        {
            var b = a;
            return (b += 30) * b;
        }

        [Fact]
        public void StoreLocalTest()
        {
            var ret = ILDecompiler.Decompile(GetMethod(nameof(StoreLocal)));

            // TODO: Test
            foreach (var s in ret)
            {
                Output?.WriteLine(s.ToString());
            }
        }

        [Fact]
        public void StoreLocalTest_Expression()
        {
            var ret = ILDecompiler.Decompile(GetMethod(nameof(StoreLocal_Expression)));

            // TODO: Test
            foreach (var s in ret)
            {
                Output?.WriteLine(s.ToString());
            }
        }

        #endregion Local

        #endregion Store

        #region Unary

        #region NotTest

        private static int Not(int a) => ~a;

        [Theory]
        [InlineData(nameof(Not))]
        public void NotTest(string methodName)
        {
            var ret = ILDecompiler.Decompile(GetMethod(methodName));

            Assert.Equal(1, ret.Count);
            Assert.True(new ParameterExpression("a", typeof(int)).Not().ToReturnStatement().IsEquivalentTo(ret[0]));
        }

        #endregion NotTest

        #region NegateTest

        private static int Negate(int a) => -a;

        [Theory]
        [InlineData(nameof(Negate))]
        public void NegateTest(string methodName)
        {
            var ret = ILDecompiler.Decompile(GetMethod(methodName));

            Assert.Equal(1, ret.Count);
            Assert.True(new ParameterExpression("a", typeof(int)).Negate().ToReturnStatement().IsEquivalentTo(ret[0]));
        }

        #endregion NegateTest

        #endregion Unary

        #region Binary

        #region Add

        private static int Add(int a) => unchecked(a + 1);

        private static int AddChecked(int a) => checked(a + 1);

        private static uint AddCheckedUnsigned(uint a) => checked(a + 1);

        [Theory]
        [InlineData(nameof(Add), BinaryOperator.Add, false)]
        [InlineData(nameof(AddChecked), BinaryOperator.AddChecked, false)]
        [InlineData(nameof(AddCheckedUnsigned), BinaryOperator.AddChecked, true)]
        public void AddTest(string methodName, BinaryOperator @operator, bool unsigned)
        {
            var ret = ILDecompiler.Decompile(GetMethod(methodName));

            Assert.Equal(1, ret.Count);
            Assert.True(new ParameterExpression("a", unsigned ? typeof(uint) : typeof(int)).MakeBinary(1.ToExpression(), @operator).ToReturnStatement().IsEquivalentTo(ret[0]));
        }

        #endregion Add

        #region Subtract

        private static int Subtract(int a) => unchecked(a - 1);

        private static int SubtractChecked(int a) => checked(a - 1);

        private static uint SubtractCheckedUnsigned(uint a) => checked(a - 1);

        [Theory]
        [InlineData(nameof(Subtract), BinaryOperator.Subtract, false)]
        [InlineData(nameof(SubtractChecked), BinaryOperator.SubtractChecked, false)]
        [InlineData(nameof(SubtractCheckedUnsigned), BinaryOperator.SubtractChecked, true)]
        public void SubtractTest(string methodName, BinaryOperator @operator, bool unsigned)
        {
            var ret = ILDecompiler.Decompile(GetMethod(methodName));

            Assert.Equal(1, ret.Count);
            Assert.True(new ParameterExpression("a", unsigned ? typeof(uint) : typeof(int)).MakeBinary(1.ToExpression(), @operator).ToReturnStatement().IsEquivalentTo(ret[0]));
        }

        #endregion Subtract

        #region Multiply

        private static int Multiply(int a) => unchecked(a * 3);

        private static int MultiplyChecked(int a) => checked(a * 3);

        private static uint MultiplyCheckedUnsigned(uint a) => checked(a * 3);

        [Theory]
        [InlineData(nameof(Multiply), BinaryOperator.Multiply, false)]
        [InlineData(nameof(MultiplyChecked), BinaryOperator.MultiplyChecked, false)]
        [InlineData(nameof(MultiplyCheckedUnsigned), BinaryOperator.MultiplyChecked, true)]
        public void MultiplyTest(string methodName, BinaryOperator @operator, bool unsigned)
        {
            var ret = ILDecompiler.Decompile(GetMethod(methodName));

            Assert.Equal(1, ret.Count);
            Assert.True(new ParameterExpression("a", unsigned ? typeof(uint) : typeof(int)).MakeBinary(3.ToExpression(), @operator).ToReturnStatement().IsEquivalentTo(ret[0]));
        }

        #endregion Multiply

        #region Divide

        private static int Divide(int a) => unchecked(a / 5);

        private static uint DivideCheckedUnsigned(uint a) => checked(a / 5);

        [Theory]
        [InlineData(nameof(Divide), false)]
        [InlineData(nameof(DivideCheckedUnsigned), true)]
        public void DivideTest(string methodName, bool unsigned)
        {
            var ret = ILDecompiler.Decompile(GetMethod(methodName));

            Assert.Equal(1, ret.Count);
            Assert.True(new ParameterExpression("a", unsigned ? typeof(uint) : typeof(int)).Divide(5.ToExpression()).ToReturnStatement().IsEquivalentTo(ret[0]));
        }

        #endregion Divide

        #endregion Binary

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

        [Fact]
        public void BranchTrueTest()
        {
            var ret = ILDecompiler.Decompile(GetMethod(nameof(BranchTrue)));

            // TODO: Test
            foreach (var s in ret)
            {
                Output?.WriteLine(s.ToString());
            }
        }

        [Fact]
        public void BranchFalseTest()
        {
            var ret = ILDecompiler.Decompile(GetMethod(nameof(BranchFalse)));

            // TODO: Test
            foreach (var s in ret)
            {
                Output?.WriteLine(s.ToString());
            }
        }

        #endregion Branch

        #region Loop

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

        [Fact]
        public void WhileTest()
        {
            var ret = ILDecompiler.Decompile(GetMethod(nameof(While)));

            // TODO: Test
            foreach (var s in ret)
            {
                Output?.WriteLine(s.ToString());
            }
        }

        [Fact]
        public void WhileInfiniteTest()
        {
            // TODO: remove unused variable assignment
            // TODO: remove goto last in infinite loop
            var ret = ILDecompiler.Decompile(GetMethod(nameof(WhileInfinite)));

            // TODO: Test
            foreach (var s in ret)
            {
                Output?.WriteLine(s.ToString());
            }
        }

        #endregion Loop
    }
}