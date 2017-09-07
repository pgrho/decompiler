using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LE = System.Linq.Expressions.Expression;

namespace Shipwreck.CSharpModels.Expressions
{
    internal sealed class BinaryEvaluator
    {
        private readonly ExpressionType Operator;
        private readonly Dictionary<int, Func<object, object, object>> _Delegates;

        public BinaryEvaluator(ExpressionType @operator)
        {
            Operator = @operator;
            _Delegates = new Dictionary<int, Func<object, object, object>>();
        }

        public object Evaluate(object value, object right)
        {
            var lt = value.GetType();
            var rt = right.GetType();
            var k = ((int)Type.GetTypeCode(lt) << 16) | ((int)Type.GetTypeCode(rt) & 0xffff);
            if (!_Delegates.TryGetValue(k, out var d))
            {
                var p1 = LE.Parameter(typeof(object));
                var p2 = LE.Parameter(typeof(object));
                d = LE.Lambda<Func<object, object, object>>(
                        LE.Convert(
                            LE.MakeBinary(
                                Operator,
                                LE.Convert(p1, lt),
                                LE.Convert(p2, rt)),
                            typeof(object)), p1, p2).Compile();
                _Delegates[k] = d;
            }

            return d(value, right);
        }
    }
}