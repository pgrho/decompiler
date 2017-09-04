using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Shipwreck.Decompiler.Expressions;
using Shipwreck.Decompiler.Statements;
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
            Assert.True(new ParameterExpression("a", type.MakeArrayType()).MakeIndex(1.ToExpression()).ToReturnStatement().IsEquivalentTo(ret[0]));
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

        #region Convert

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

            var ret = ILDecompiler.Decompile(m);

            var ue = Assert.IsType<UnaryExpression>(Assert.IsType<ReturnStatement>(ret.Single()).Value);

            Assert.Equal(isChecked ? UnaryOperator.ConvertChecked : UnaryOperator.Convert, ue.Operator);
            Assert.True(ue.IsEquivalentTo(c));
        }

        #endregion Convert

        #region Unary

        #region OnesComplementTest

        private static int OnesComplement(int a) => ~a;

        [Theory]
        [InlineData(nameof(OnesComplement))]
        public void OnesComplementTest(string methodName)
        {
            var ret = ILDecompiler.Decompile(GetMethod(methodName));

            Assert.Equal(1, ret.Count);
            Assert.True(new ParameterExpression("a", typeof(int)).OnesComplement().ToReturnStatement().IsEquivalentTo(ret[0]));
        }

        #endregion OnesComplementTest

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

        #region IL Instruction

        private static int Add(int l, byte r)
            => l + r;

        private static int AddChecked(int l, byte r)
            => checked(l + r);

        private static uint AddCheckedUnsigned(uint l, byte r)
            => checked(l + r);

        private static int Subtract(int l, byte r)
            => l - r;

        private static int SubtractChecked(int l, byte r)
            => checked(l - r);

        private static uint SubtractCheckedUnsigned(uint l, byte r)
            => checked(l - r);

        private static int Multiply(int l, byte r)
            => l * r;

        private static int MultiplyChecked(int l, byte r)
            => checked(l * r);

        private static uint MultiplyCheckedUnsigned(uint l, byte r)
            => checked(l * r);

        private static int Divide(int l, byte r)
            => l / r;

        private static uint DivideUnsigned(uint l, byte r)
            => l / r;

        private static int Modulo(int l, byte r)
            => l % r;

        private static uint ModuloUnsigned(uint l, byte r)
            => l % r;

        private static int BitwiseAnd(int l, byte r)
            => l & r;

        private static int BitwiseOr(int l, byte r)
            => l | r;

        private static int ExclusiveOr(int l, byte r)
            => l ^ r;

        private static int LeftShift(int l, byte r)
            => l << r;

        private static int RightShift(int l, byte r)
            => l >> r;

        private static uint RightShiftUnsigned(uint l, byte r)
            => l >> r;

        #endregion IL Instruction

        #region Custom Operator Overloading

        private static Zero AddCustom(Zero l, Zero r)
            => l + r;

        private static Zero SubtractCustom(Zero l, Zero r)
            => l - r;

        private static Zero MultiplyCustom(Zero l, Zero r)
            => l * r;

        private static Zero DivideCustom(Zero l, Zero r)
            => l / r;

        private static Zero ModuloCustom(Zero l, Zero r)
            => l % r;

        private static Zero BitwiseAndCustom(Zero l, Zero r)
            => l & r;

        private static Zero BitwiseOrCustom(Zero l, Zero r)
            => l | r;

        private static Zero ExclusiveOrCustom(Zero l, Zero r)
            => l ^ r;

        private static Zero LeftShiftCustom(Zero l, byte r)
            => l << r;

        private static Zero RightShiftCustom(Zero l, byte r)
            => l >> r;

        #endregion Custom Operator Overloading

        [Theory]
        [InlineData(nameof(Add), BinaryOperator.Add)]
        [InlineData(nameof(AddChecked), BinaryOperator.AddChecked)]
        [InlineData(nameof(AddCheckedUnsigned), BinaryOperator.AddChecked)]
        [InlineData(nameof(Subtract), BinaryOperator.Subtract)]
        [InlineData(nameof(SubtractChecked), BinaryOperator.SubtractChecked)]
        [InlineData(nameof(SubtractCheckedUnsigned), BinaryOperator.SubtractChecked)]
        [InlineData(nameof(Multiply), BinaryOperator.Multiply)]
        [InlineData(nameof(MultiplyChecked), BinaryOperator.MultiplyChecked)]
        [InlineData(nameof(MultiplyCheckedUnsigned), BinaryOperator.MultiplyChecked)]
        [InlineData(nameof(Divide), BinaryOperator.Divide)]
        [InlineData(nameof(DivideUnsigned), BinaryOperator.Divide)]
        [InlineData(nameof(Modulo), BinaryOperator.Modulo)]
        [InlineData(nameof(ModuloUnsigned), BinaryOperator.Modulo)]
        [InlineData(nameof(BitwiseAnd), BinaryOperator.And)]
        [InlineData(nameof(BitwiseOr), BinaryOperator.Or)]
        [InlineData(nameof(ExclusiveOr), BinaryOperator.ExclusiveOr)]
        [InlineData(nameof(AddCustom), BinaryOperator.Add)]
        [InlineData(nameof(SubtractCustom), BinaryOperator.Subtract)]
        [InlineData(nameof(MultiplyCustom), BinaryOperator.Multiply)]
        [InlineData(nameof(DivideCustom), BinaryOperator.Divide)]
        [InlineData(nameof(ModuloCustom), BinaryOperator.Modulo)]
        [InlineData(nameof(BitwiseAndCustom), BinaryOperator.And)]
        [InlineData(nameof(BitwiseOrCustom), BinaryOperator.Or)]
        [InlineData(nameof(ExclusiveOrCustom), BinaryOperator.ExclusiveOr)]
        [InlineData(nameof(LeftShiftCustom), BinaryOperator.LeftShift)]
        [InlineData(nameof(RightShiftCustom), BinaryOperator.RightShift)]
        public void TestBinary(string methodName, BinaryOperator op)
        {
            var m = GetMethod(methodName);
            var t = m.ReturnType;

            AssertMethod(
                m,
                new ParameterExpression("l", t)
                    .MakeBinary(new ParameterExpression("r", m.GetParameters()[1].ParameterType), op)
                    .ToReturnStatement());
        }

        [Theory]
        [InlineData(nameof(LeftShift), BinaryOperator.LeftShift)]
        [InlineData(nameof(RightShift), BinaryOperator.RightShift)]
        [InlineData(nameof(RightShiftUnsigned), BinaryOperator.RightShift)]
        public void TestShift(string methodName, BinaryOperator op)
        {
            var m = GetMethod(methodName);
            var t = m.ReturnType;

            // TODO: reduce (right & 31) to right
            AssertMethod(
                m,
                new ParameterExpression("l", t)
                    .MakeBinary(new ParameterExpression("r", typeof(byte)).And(31.ToExpression()), op)
                    .ToReturnStatement());
        }

        #endregion Binary

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

        #region Call

        private static IntPtr Call() => NewObjectIntPtr();

        private string CallVirt() => ToString();

        private string CallBase() => base.ToString();

        [Theory]
        [InlineData(nameof(Call))]
        [InlineData(nameof(CallVirt))]
        [InlineData(nameof(CallBase))]
        public void CallTest(string methodName)
        {
            AssertMethod(GetMethod(methodName));
        }

        #endregion Call

        #region Property

        private static int GetProperty(StringBuilder a)
            => a.Length;

        private static void SetProperty(StringBuilder a, int v)
            => a.Length = v;

        private static char GetIndexedProperty(StringBuilder a)
            => a[0];

        private static void SetIndexedProperty(StringBuilder a, char v)
            => a[0] = v;

        [Fact]
        public void GetPropertyTest()
            => AssertMethod(
                GetMethod(nameof(GetProperty)),
                new ParameterExpression("a", typeof(StringBuilder))
                    .Property(typeof(StringBuilder).GetProperty(nameof(StringBuilder.Length)))
                    .ToReturnStatement());

        [Fact]
        public void SetPropertyTest()
            => AssertMethod(
                GetMethod(nameof(SetProperty)),
                new ParameterExpression("a", typeof(StringBuilder))
                    .Property(typeof(StringBuilder).GetProperty(nameof(StringBuilder.Length)))
                    .Assign(new ParameterExpression("v", typeof(int)))
                    .ToReturnStatement());

        [Fact]
        public void GetIndexedPropertyTest()
            => AssertMethod(
                GetMethod(nameof(GetIndexedProperty)),
                new ParameterExpression("a", typeof(StringBuilder))
                    .MakeIndex(0.ToExpression())
                    .ToReturnStatement());

        [Fact]
        public void SetIndexedPropertyTest()
            => AssertMethod(
                GetMethod(nameof(SetIndexedProperty)),
                new ParameterExpression("a", typeof(StringBuilder))
                    .MakeIndex(0.ToExpression())
                    .Assign(new ParameterExpression("v", typeof(char)))
                    .ToReturnStatement());

        #endregion Property

        #region Event

        private static void AddEvent(Process p, EventHandler h)
            => p.Exited += h;

        private static void RemoveEvent(Process p, EventHandler h)
            => p.Exited -= h;

        [Theory]
        [InlineData(nameof(AddEvent), true)]
        [InlineData(nameof(RemoveEvent), false)]
        public void EventTest(string methodName, bool isAdd)
            => AssertMethod(
                GetMethod(methodName),
                new ParameterExpression("p", typeof(Process))
                    .MakeMemberAccess(typeof(Process).GetEvent(nameof(Process.Exited)))
                    .Assign(new ParameterExpression("h", typeof(EventHandler)), isAdd ? BinaryOperator.Add : BinaryOperator.Subtract)
                    .ToReturnStatement());

        #endregion Event

        #region Operator

        private struct Zero
        {
            #region Unary

            public static Zero operator +(Zero l)
                => l;

            public static Zero operator -(Zero l)
                => l;

            public static Zero operator !(Zero l)
                => l;

            public static Zero operator ~(Zero l)
                => l;

            public static Zero operator ++(Zero l)
                => l;

            public static Zero operator --(Zero l)
                => l;

            public static bool operator true(Zero l)
                => false;

            public static bool operator false(Zero l)
                => true;

            #endregion Unary

            #region Comparison

            public static bool operator ==(Zero l, Zero r)
                => true;

            public static bool operator !=(Zero l, Zero r)
                => false;

            public static bool operator <(Zero l, Zero r)
                => false;

            public static bool operator >(Zero l, Zero r)
                => false;

            public static bool operator <=(Zero l, Zero r)
                => false;

            public static bool operator >=(Zero l, Zero r)
                => false;

            #endregion Comparison

            #region Arithmetic

            public static Zero operator +(Zero l, Zero r)
                => default(Zero);

            public static Zero operator -(Zero l, Zero r)
                => default(Zero);

            public static Zero operator *(Zero l, Zero r)
                => default(Zero);

            public static Zero operator /(Zero l, Zero r)
                => default(Zero);

            public static Zero operator %(Zero l, Zero r)
                => default(Zero);

            #endregion Arithmetic

            #region bitwise

            public static Zero operator &(Zero l, Zero r)
                => default(Zero);

            public static Zero operator |(Zero l, Zero r)
                => default(Zero);

            public static Zero operator ^(Zero l, Zero r)
                => default(Zero);

            public static Zero operator <<(Zero l, int r)
                => default(Zero);

            public static Zero operator >>(Zero l, int r)
                => default(Zero);

            #endregion bitwise

            public override bool Equals(object obj)
                => obj is Zero;

            public override int GetHashCode()
                => 0;

            public override string ToString()
                => "{zero}";
        }

        #endregion Operator

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

        #region Try-Catch

        private static int TryCatch(int a, int b)
        {
            try
            {
                return checked(a + b);
            }
            catch (InvalidOperationException)
            {
                return -3;
            }
            catch
            {
                try
                {
                    return checked(a - b);
                }
                catch (InvalidOperationException)
                {
                    return -2;
                }
                catch
                {
                    return -1;
                }
            }
        }

        [Fact]
        public void TryCatchTest()
        {
            AssertMethod(GetMethod(nameof(TryCatch)));
        }

        #endregion Try-Catch

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
        [InlineData(nameof(Continue))]
        public void ContinueTest(string methodName)
            => AssertMethod(GetMethod(methodName));

        private void AssertMethod(MethodInfo method, params Statement[] expectedStatement)
        {
            List<Statement> dm;
            try
            {
                dm = ILDecompiler.Decompile(method);
            }
            catch
            {
                throw;
            }

            if (expectedStatement.Any())
            {
                try
                {
                    Assert.Equal(expectedStatement.Length, dm.Count);
                    for (int i = 0; i < dm.Count; i++)
                    {
                        Assert.True(expectedStatement[i].IsEquivalentTo(dm[i]));
                    }
                }
                catch
                {
                    WriteStatements(dm);
                    throw;
                }
            }
            else
            {
                WriteStatements(dm);
            }
        }

        private void WriteStatements(List<Statement> dm)
        {
            if (Output != null)
            {
                using (var sw = new StringWriter())
                using (var tw = new IndentedTextWriter(sw))
                {
                    foreach (var s in dm)
                    {
                        s.WriteTo(tw);
                    }

                    tw.Flush();

                    Output.WriteLine(sw.ToString());
                }
            }
        }
    }
}