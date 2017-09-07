using Shipwreck.CSharpModels.Expressions;
using Xunit;
using Xunit.Abstractions;

namespace Shipwreck.Decompiler.ILDecompilerTests
{
    public sealed class BinaryExpressionTest : ILDecompilerTestBase
    {
        public BinaryExpressionTest(ITestOutputHelper output = null)
            : base(output)
        {
        }

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

        private static string NullCoalesce(string l, string r) => l ?? r;

        private static int NullCoalesceNullable(int? l, int r) => l ?? r;

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
        [InlineData(nameof(NullCoalesce), BinaryOperator.NullCoalesce)]
        // TODO: [InlineData(nameof(NullCoalesceNullable), BinaryOperator.NullCoalesce)]
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
        public void BinaryTest(string name, BinaryOperator op)
        {
            var m = GetMethod(name);
            var ps = m.GetParameters();
            AssertMethod(
                m,
                new ParameterExpression("l", ps[0].ParameterType)
                    .MakeBinary(new ParameterExpression("r", ps[1].ParameterType), op)
                    .ToReturnStatement());
        }
    }
}