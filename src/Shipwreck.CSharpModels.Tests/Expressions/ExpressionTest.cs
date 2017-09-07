using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Shipwreck.CSharpModels.Expressions
{
    public class ExpressionTest
    {
        [Fact]
        public void EnumeratePostOrderTest()
        {
            var e = (1.ToExpression().Multiply(2.ToExpression())).Add(3.ToExpression().Divide(4.ToExpression()));
            var actual = e.EnumeratePostOrder().Select(i => i.ToString()).ToArray();
            Assert.Equal(new[] { "1", "2", "1 * 2", "3", "4", "3 / 4", "1 * 2 + 3 / 4" }, actual);
        }
    }
}
