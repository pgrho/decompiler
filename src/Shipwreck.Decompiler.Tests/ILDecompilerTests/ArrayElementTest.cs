using System;
using Shipwreck.CSharpModels.Expressions;
using Xunit;
using Xunit.Abstractions;

namespace Shipwreck.Decompiler.ILDecompilerTests
{
    public sealed class ArrayElementTest : ILDecompilerTestBase
    {
        public ArrayElementTest(ITestOutputHelper output = null)
            : base(output)
        {
        }

        #region Load Element

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
        public void LoadElementTest(string m, Type t)
            => AssertMethod(GetMethod(m), new ParameterExpression("a", t.MakeArrayType()).MakeIndex(1.ToExpression()).ToReturnStatement());

        #endregion Load Element

        #region Store Element

        private static void StoreElement(DateTime[] a) => a[1] = new DateTime(1, 2, 3);

        private static void StoreElementDefault(DateTime[] a) => a[1] = default(DateTime);

        private static void StoreElementDefaultExpression(DateTime[] a) => DateTime.Compare(DateTime.MinValue, a[1] = default(DateTime));

        private static unsafe void StoreElementPointer(void*[] a) => a[1] = (void*)0;

        private static void StoreElementByte(byte[] a) => a[1] = 0;

        private static void StoreElementInt16(short[] a) => a[1] = 0;

        private static void StoreElementInt32(int[] a) => a[1] = 0;

        private static void StoreElementInt64(long[] a) => a[1] = 0;

        private static void StoreElementSingle(float[] a) => a[1] = 0;

        private static void StoreElementDouble(double[] a) => a[1] = 0;

        private static void StoreElementRef(string[] a) => a[1] = null;

        [Theory]
        [InlineData(nameof(StoreElement))]
        [InlineData(nameof(StoreElementDefault))]
        //TODO: [InlineData(nameof(StoreElementDefaultExpression))]
        [InlineData(nameof(StoreElementPointer))]
        [InlineData(nameof(StoreElementByte))]
        [InlineData(nameof(StoreElementInt16))]
        [InlineData(nameof(StoreElementInt32))]
        [InlineData(nameof(StoreElementInt64))]
        [InlineData(nameof(StoreElementSingle))]
        [InlineData(nameof(StoreElementDouble))]
        [InlineData(nameof(StoreElementRef))]
        public void NotImplementedTest(string m)
            => AssertMethod(GetMethod(m));

        #endregion Store Element
    }
}