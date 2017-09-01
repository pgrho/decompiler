using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public abstract class Expression : Syntax
    {
        public abstract void WriteTo(TextWriter writer);

        public override string ToString()
        {
            using (var sw = new StringWriter())
            {
                WriteTo(sw);
                return sw.ToString();
            }
        }
    }
}