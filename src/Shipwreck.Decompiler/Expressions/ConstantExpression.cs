using System;
using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class ConstantExpression : Expression
    {
        internal ConstantExpression(object value, Type type = null)
        {
            Value = value;
            Type = type ?? value?.GetType() ?? typeof(object);
        }

        public object Value { get; }

        public override Type Type { get; }

        public override bool IsEquivalentTo(Syntax other)
            => other is ConstantExpression ce && Equals(Value, ce.Value);

        public override void WriteTo(TextWriter writer)
        {
            if (Value == null)
            {
                writer.Write("null");
            }
            else if (Value is bool b)
            {
                writer.Write(b ? "true" : "false");
            }
            else if (Value is double d)
            {
                writer.Write(d.ToString("R"));
            }
            else if (Value is float f)
            {
                writer.Write(f.ToString("R"));
            }
            else if (Value is string s)
            {
                writer.Write('"');
                foreach (var c in s)
                {
                    switch (c)
                    {
                        case '\0':
                            writer.Write("\\0");
                            break;

                        case '\b':
                            writer.Write("\\b");
                            break;

                        case '\n':
                            writer.Write("\\n");
                            break;

                        case '\r':
                            writer.Write("\\r");
                            break;

                        case '\t':
                            writer.Write("\\t");
                            break;

                        case '"':
                            writer.Write("\\\"");
                            break;

                        default:
                            writer.Write(c);
                            break;
                    }
                }
                writer.Write('"');
            }
            else if (Value is IFormattable c)
            {
                writer.Write(c.ToString("D", null));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public override ExpressionPrecedence Precedence
            => ExpressionPrecedence.Primary;
    }
}