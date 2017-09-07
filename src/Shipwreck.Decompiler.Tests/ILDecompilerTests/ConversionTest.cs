using System;
using Shipwreck.CSharpModels.Expressions;
using Xunit;
using Xunit.Abstractions;

namespace Shipwreck.Decompiler.ILDecompilerTests
{
    public sealed class ConversionTest : ILDecompilerTestBase
    {
        public ConversionTest(ITestOutputHelper output = null)
            : base(output)
        {
        }

        private static SByte ConvertSByte(float a) => (SByte)a;

        private static Int16 ConvertInt16(float a) => (Int16)a;

        private static Int32 ConvertInt32(float a) => (Int32)a;

        private static Int64 ConvertInt64(float a) => (Int64)a;

        private static Byte ConvertByte(float a) => (Byte)a;

        private static UInt16 ConvertUInt16(float a) => (UInt16)a;

        private static UInt32 ConvertUInt32(float a) => (UInt32)a;

        private static UInt64 ConvertUInt64(float a) => (UInt64)a;

        private static SByte ConvertSByteChecked(float a) => checked((SByte)a);

        private static Int16 ConvertInt16Checked(float a) => checked((Int16)a);

        private static Int32 ConvertInt32Checked(float a) => checked((Int32)a);

        private static Int64 ConvertInt64Checked(float a) => checked((Int64)a);

        private static Byte ConvertByteChecked(float a) => checked((Byte)a);

        private static UInt16 ConvertUInt16Checked(float a) => checked((UInt16)a);

        private static UInt32 ConvertUInt32Checked(float a) => checked((UInt32)a);

        private static UInt64 ConvertUInt64Checked(float a) => checked((UInt64)a);

        private static SByte ConvertSByteUnsignedChecked(ulong a) => checked((SByte)a);

        private static Int16 ConvertInt16UnsignedChecked(ulong a) => checked((Int16)a);

        private static Int32 ConvertInt32UnsignedChecked(ulong a) => checked((Int32)a);

        private static Int64 ConvertInt64UnsignedChecked(ulong a) => checked((Int64)a);

        private static Byte ConvertByteUnsignedChecked(ulong a) => checked((Byte)a);

        private static UInt16 ConvertUInt16UnsignedChecked(ulong a) => checked((UInt16)a);

        private static UInt32 ConvertUInt32UnsignedChecked(ulong a) => checked((UInt32)a);

        private static UInt64 ConvertUInt64UnsignedChecked(uint a) => checked((UInt64)a);

        private static float ConvertSingle(int a) => a;

        private static double ConvertDouble(int a) => a;

        private static float ConvertFloatUnsigned(ulong a) => a;

        [Theory]
        // TODO: test conv.i
        // TODO: test conv.ovf.i
        // TODO: test conv.ovf.i.un
        [InlineData(nameof(ConvertSByte))]
        [InlineData(nameof(ConvertInt16))]
        [InlineData(nameof(ConvertInt32))]
        [InlineData(nameof(ConvertInt64))]
        [InlineData(nameof(ConvertByte))]
        [InlineData(nameof(ConvertUInt16))]
        [InlineData(nameof(ConvertUInt32))]
        [InlineData(nameof(ConvertUInt64))]
        [InlineData(nameof(ConvertSByteChecked))]
        [InlineData(nameof(ConvertInt16Checked))]
        [InlineData(nameof(ConvertInt32Checked))]
        [InlineData(nameof(ConvertInt64Checked))]
        [InlineData(nameof(ConvertByteChecked))]
        [InlineData(nameof(ConvertUInt16Checked))]
        [InlineData(nameof(ConvertUInt32Checked))]
        [InlineData(nameof(ConvertUInt64Checked))]
        [InlineData(nameof(ConvertSByteUnsignedChecked))]
        [InlineData(nameof(ConvertInt16UnsignedChecked))]
        [InlineData(nameof(ConvertInt32UnsignedChecked))]
        [InlineData(nameof(ConvertInt64UnsignedChecked))]
        [InlineData(nameof(ConvertByteUnsignedChecked))]
        [InlineData(nameof(ConvertUInt16UnsignedChecked))]
        [InlineData(nameof(ConvertUInt32UnsignedChecked))]
        // TODO: Test conv.ovf.u8.un [InlineData(nameof(ConvertUInt64UnsignedChecked))]
        [InlineData(nameof(ConvertSingle))]
        [InlineData(nameof(ConvertDouble))]
        // TODO: test conv.r.un
        //[InlineData(nameof(ConvertFloatUnsigned))]
        public void ConvertTest(string methodName)
        {
            var m = GetMethod(methodName);

            var p = m.GetParameters()[0];

            var isChecked = m.Name.EndsWith("Checked");
            var pe = new ParameterExpression(p.Name, p.ParameterType);
            var c = m.Name.EndsWith("Checked") ? pe.ConvertChecked(m.ReturnType) : pe.Convert(m.ReturnType);

            AssertMethod(m, c.ToReturnStatement());
        }
    }
}