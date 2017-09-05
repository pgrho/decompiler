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
            Assert.True(new ThisExpression(GetType()).ToReturnStatement().IsEquivalentTo(ret[0]));
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
        [InlineData(nameof(LeftShift), BinaryOperator.LeftShift)]
        [InlineData(nameof(RightShift), BinaryOperator.RightShift)]
        [InlineData(nameof(RightShiftUnsigned), BinaryOperator.RightShift)]
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
        public void BinaryTest(string methodName, BinaryOperator op)
        {
            var m = GetMethod(methodName);
            var t = m.ReturnType;

            AssertMethod(
                m,
                new ParameterExpression("l", t)
                    .MakeBinary(new ParameterExpression("r", m.GetParameters()[1].ParameterType), op)
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
                    .ToStatement());

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
                    .ToStatement());

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
                    .ToStatement());

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

        [Fact]
        public void SwitchTest()
        {
            AssertMethod(GetMethod(nameof(Switch)));
        }

        #endregion Switch

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
            finally
            {
                Console.WriteLine("{0} {1}", a.ToString(), b.ToString());
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

        private static void Lock(IDisposable d)
        {
            lock (d)
            {
                d.Dispose();
            }
        }

        private static void Using()
        {
            using (var ms = new MemoryStream())
            {
                ms.Flush();
            }
        }

        [Theory]
        [InlineData(nameof(Continue))]
        // TODO: [InlineData(nameof(Lock))]
        [InlineData(nameof(Using))]
        [InlineData(nameof(LoadLength))]
        [InlineData(nameof(LoadLocalAddressShort))]
        [InlineData(nameof(LoadLocalAddress))]
        [InlineData(nameof(LoadStaticField))]
        [InlineData(nameof(LoadStaticFieldAddess))]
        [InlineData(nameof(LoadFieldAddess))]
        [InlineData(nameof(NewArray))]
        public void MiscTest(string methodName)
            => AssertMethod(GetMethod(methodName));

        private static int LoadLength(int[] a) => a.Length;

        private static int LoadLocalAddressShort()
        {
            if (int.TryParse("0", out var v))
            {
                return v;
            }
            return 0;
        }

        private static int LoadLocalAddress()
        {
            var l0 = int.MinValue; var l1 = l0 + 1; var l2 = l1 + 2; var l3 = l2 + 3; var l4 = l3 + 4; var l5 = l4 + 5; var l6 = l5 + 6; var l7 = l6 + 7; var l8 = l7 + 8; var l9 = l8 + 9; var l10 = l9 + 10; var l11 = l10 + 11; var l12 = l11 + 12; var l13 = l12 + 13; var l14 = l13 + 14; var l15 = l14 + 15; var l16 = l15 + 16; var l17 = l16 + 17; var l18 = l17 + 18; var l19 = l18 + 19; var l20 = l19 + 20; var l21 = l20 + 21; var l22 = l21 + 22; var l23 = l22 + 23; var l24 = l23 + 24; var l25 = l24 + 25; var l26 = l25 + 26; var l27 = l26 + 27; var l28 = l27 + 28; var l29 = l28 + 29; var l30 = l29 + 30; var l31 = l30 + 31; var l32 = l31 + 32; var l33 = l32 + 33; var l34 = l33 + 34; var l35 = l34 + 35; var l36 = l35 + 36; var l37 = l36 + 37; var l38 = l37 + 38; var l39 = l38 + 39; var l40 = l39 + 40; var l41 = l40 + 41; var l42 = l41 + 42; var l43 = l42 + 43; var l44 = l43 + 44; var l45 = l44 + 45; var l46 = l45 + 46; var l47 = l46 + 47; var l48 = l47 + 48; var l49 = l48 + 49; var l50 = l49 + 50; var l51 = l50 + 51; var l52 = l51 + 52; var l53 = l52 + 53; var l54 = l53 + 54; var l55 = l54 + 55; var l56 = l55 + 56; var l57 = l56 + 57; var l58 = l57 + 58; var l59 = l58 + 59; var l60 = l59 + 60; var l61 = l60 + 61; var l62 = l61 + 62; var l63 = l62 + 63; var l64 = l63 + 64; var l65 = l64 + 65; var l66 = l65 + 66; var l67 = l66 + 67; var l68 = l67 + 68; var l69 = l68 + 69; var l70 = l69 + 70; var l71 = l70 + 71; var l72 = l71 + 72; var l73 = l72 + 73; var l74 = l73 + 74; var l75 = l74 + 75; var l76 = l75 + 76; var l77 = l76 + 77; var l78 = l77 + 78; var l79 = l78 + 79; var l80 = l79 + 80; var l81 = l80 + 81; var l82 = l81 + 82; var l83 = l82 + 83; var l84 = l83 + 84; var l85 = l84 + 85; var l86 = l85 + 86; var l87 = l86 + 87; var l88 = l87 + 88; var l89 = l88 + 89; var l90 = l89 + 90; var l91 = l90 + 91; var l92 = l91 + 92; var l93 = l92 + 93; var l94 = l93 + 94; var l95 = l94 + 95; var l96 = l95 + 96; var l97 = l96 + 97; var l98 = l97 + 98; var l99 = l98 + 99; var l100 = l99 + 100; var l101 = l100 + 101; var l102 = l101 + 102; var l103 = l102 + 103; var l104 = l103 + 104; var l105 = l104 + 105; var l106 = l105 + 106; var l107 = l106 + 107; var l108 = l107 + 108; var l109 = l108 + 109; var l110 = l109 + 110; var l111 = l110 + 111; var l112 = l111 + 112; var l113 = l112 + 113; var l114 = l113 + 114; var l115 = l114 + 115; var l116 = l115 + 116; var l117 = l116 + 117; var l118 = l117 + 118; var l119 = l118 + 119; var l120 = l119 + 120; var l121 = l120 + 121; var l122 = l121 + 122; var l123 = l122 + 123; var l124 = l123 + 124; var l125 = l124 + 125; var l126 = l125 + 126; var l127 = l126 + 127; var l128 = l127 + 128; var l129 = l128 + 129; var l130 = l129 + 130; var l131 = l130 + 131; var l132 = l131 + 132; var l133 = l132 + 133; var l134 = l133 + 134; var l135 = l134 + 135; var l136 = l135 + 136; var l137 = l136 + 137; var l138 = l137 + 138; var l139 = l138 + 139; var l140 = l139 + 140; var l141 = l140 + 141; var l142 = l141 + 142; var l143 = l142 + 143; var l144 = l143 + 144; var l145 = l144 + 145; var l146 = l145 + 146; var l147 = l146 + 147; var l148 = l147 + 148; var l149 = l148 + 149; var l150 = l149 + 150; var l151 = l150 + 151; var l152 = l151 + 152; var l153 = l152 + 153; var l154 = l153 + 154; var l155 = l154 + 155; var l156 = l155 + 156; var l157 = l156 + 157; var l158 = l157 + 158; var l159 = l158 + 159; var l160 = l159 + 160; var l161 = l160 + 161; var l162 = l161 + 162; var l163 = l162 + 163; var l164 = l163 + 164; var l165 = l164 + 165; var l166 = l165 + 166; var l167 = l166 + 167; var l168 = l167 + 168; var l169 = l168 + 169; var l170 = l169 + 170; var l171 = l170 + 171; var l172 = l171 + 172; var l173 = l172 + 173; var l174 = l173 + 174; var l175 = l174 + 175; var l176 = l175 + 176; var l177 = l176 + 177; var l178 = l177 + 178; var l179 = l178 + 179; var l180 = l179 + 180; var l181 = l180 + 181; var l182 = l181 + 182; var l183 = l182 + 183; var l184 = l183 + 184; var l185 = l184 + 185; var l186 = l185 + 186; var l187 = l186 + 187; var l188 = l187 + 188; var l189 = l188 + 189; var l190 = l189 + 190; var l191 = l190 + 191; var l192 = l191 + 192; var l193 = l192 + 193; var l194 = l193 + 194; var l195 = l194 + 195; var l196 = l195 + 196; var l197 = l196 + 197; var l198 = l197 + 198; var l199 = l198 + 199; var l200 = l199 + 200; var l201 = l200 + 201; var l202 = l201 + 202; var l203 = l202 + 203; var l204 = l203 + 204; var l205 = l204 + 205; var l206 = l205 + 206; var l207 = l206 + 207; var l208 = l207 + 208; var l209 = l208 + 209; var l210 = l209 + 210; var l211 = l210 + 211; var l212 = l211 + 212; var l213 = l212 + 213; var l214 = l213 + 214; var l215 = l214 + 215; var l216 = l215 + 216; var l217 = l216 + 217; var l218 = l217 + 218; var l219 = l218 + 219; var l220 = l219 + 220; var l221 = l220 + 221; var l222 = l221 + 222; var l223 = l222 + 223; var l224 = l223 + 224; var l225 = l224 + 225; var l226 = l225 + 226; var l227 = l226 + 227; var l228 = l227 + 228; var l229 = l228 + 229; var l230 = l229 + 230; var l231 = l230 + 231; var l232 = l231 + 232; var l233 = l232 + 233; var l234 = l233 + 234; var l235 = l234 + 235; var l236 = l235 + 236; var l237 = l236 + 237; var l238 = l237 + 238; var l239 = l238 + 239; var l240 = l239 + 240; var l241 = l240 + 241; var l242 = l241 + 242; var l243 = l242 + 243; var l244 = l243 + 244; var l245 = l244 + 245; var l246 = l245 + 246; var l247 = l246 + 247; var l248 = l247 + 248; var l249 = l248 + 249; var l250 = l249 + 250; var l251 = l250 + 251; var l252 = l251 + 252; var l253 = l252 + 253; var l254 = l253 + 254; var l255 = l254 + 255; var l256 = l255 + 256;

            if (int.TryParse("0", out l256))
            {
                return l256;
            }
            return 0;
        }

        private static string LoadStaticField() => bool.TrueString;

        private static int _StaticInt;

        private static int LoadStaticFieldAddess()
        {
            if (int.TryParse("0", out _StaticInt))
            {
                return _StaticInt;
            }
            return 0;
        }

        private int _InstanceInt;

        private int LoadFieldAddess()
        {
            if (int.TryParse("0", out _InstanceInt))
            {
                return _InstanceInt;
            }
            return 0;
        }

        private static int[] NewArray() => new int[0];

        #region StoreElement

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
        public void StoreElementTest(string methodName)
            => AssertMethod(GetMethod(methodName));

        #endregion StoreElement

        // TODO: starg

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