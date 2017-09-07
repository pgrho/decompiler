using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LE = System.Linq.Expressions.Expression;

namespace Shipwreck.CSharpModels.Expressions
{
    internal sealed class UnaryEvaluator
    {
        private readonly ExpressionType Operator;
        private readonly Dictionary<TypeCode, Func<object, object>> _Delegates;

        public UnaryEvaluator(ExpressionType @operator)
        {
            Operator = @operator;
            _Delegates = new Dictionary<TypeCode, Func<object, object>>();
        }

        public object Evaluate(object value)
        {
            var t = value.GetType();
            var k = Type.GetTypeCode(t);
            if (!_Delegates.TryGetValue(k, out var d))
            {
                var p = LE.Parameter(typeof(object));
                d = LE.Lambda<Func<object, object>>(LE.Convert(LE.MakeUnary(Operator, LE.Convert(p, t), null), typeof(object)), p).Compile();
                _Delegates[k] = d;
            }

            return d(value);
        }
    }
}