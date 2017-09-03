using System.IO;

namespace Shipwreck.Decompiler.Expressions
{
    public sealed class ArrayIndexExpression : Expression
    {
        public ArrayIndexExpression(Expression array, Expression index)
        {
            array.ArgumentIsNotNull(nameof(array));
            index.ArgumentIsNotNull(nameof(index));

            Array = array;
            Index = index;
        }

        public Expression Array { get; }
        public Expression Index { get; }

        public override bool IsEquivalentTo(Syntax other)
            => other is ArrayIndexExpression aie
                && Array.IsEquivalentTo(aie.Array)
                && Index.IsEquivalentTo(aie.Index);

        public override void WriteTo(TextWriter writer)
        {
            writer.Write('(');
            Array.WriteTo(writer);
            writer.Write(")[");
            Index.WriteTo(writer);
            writer.Write(']');
        }

        internal override Expression ReduceCore()
        {
            if (Array.TryReduce(out var a) | Index.TryReduce(out var i))
            {
                return new ArrayIndexExpression(a, i);
            }

            return base.ReduceCore();
        }
    }
}