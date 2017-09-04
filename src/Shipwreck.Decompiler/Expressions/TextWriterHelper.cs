using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    internal static class TextWriterHelper
    {
        public static void WriteFirstChild(this TextWriter writer, Expression expression, Expression parent)
        {
            if (expression != null)
            {
                var wrap = expression.Precedence > parent.Precedence;

                if (wrap)
                {
                    writer.Write('(');
                }
                expression.WriteTo(writer);
                if (wrap)
                {
                    writer.Write(')');
                }
            }
        }

        public static void WriteSecondChild(this TextWriter writer, Expression expression, Expression parent)
        {
            if (expression != null)
            {
                var wrap = expression.Precedence >= parent.Precedence;

                if (wrap)
                {
                    writer.Write('(');
                }
                expression.WriteTo(writer);
                if (wrap)
                {
                    writer.Write(')');
                }
            }
        }
    }
}