using System;
using System.Diagnostics;
using System.Text;
using Shipwreck.Decompiler.Expressions;
using Xunit;
using Xunit.Abstractions;

namespace Shipwreck.Decompiler.ILDecompilerTests
{
    public sealed class MemberAccessTest : ILDecompilerTestBase
    {
        public MemberAccessTest(ITestOutputHelper output = null)
            : base(output)
        {
        }

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

        private static int LoadLength(int[] a) => a.Length;

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

        [Theory]
        [InlineData(nameof(LoadLength))]
        [InlineData(nameof(LoadStaticField))]
        [InlineData(nameof(LoadStaticFieldAddess))]
        [InlineData(nameof(LoadFieldAddess))]
        public void NotImplementedTest(string m)
            => AssertMethod(GetMethod(m));
    }
}