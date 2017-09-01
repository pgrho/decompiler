using System.CodeDom.Compiler;
using System.IO;

namespace Shipwreck.Decompiler.Statements
{
    public abstract class Statement : Syntax
    {
        public abstract void WriteTo(IndentedTextWriter writer);

        public override string ToString()
        {
            using (var sw = new StringWriter())
            using (var tw = new IndentedTextWriter(sw))
            {
                WriteTo(tw);

                tw.Flush();

                return sw.ToString();
            }
        }
    }
}