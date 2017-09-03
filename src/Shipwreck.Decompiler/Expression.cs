using System.IO;

namespace Shipwreck.Decompiler
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

        public Expression Reduce()
        {
            var r = ReduceCore();
            return r == this ? this : r.Reduce();
        }

        public bool TryReduce(out Expression expression)
        {
            expression = Reduce();
            return expression != this;
        }

        internal virtual Expression ReduceCore()
            => this;
    }
}