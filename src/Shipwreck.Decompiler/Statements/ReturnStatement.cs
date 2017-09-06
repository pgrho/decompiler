using System.CodeDom.Compiler;
using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class ReturnStatement : Statement, IBreakingStatement
    {
        public ReturnStatement()
        {
        }

        public ReturnStatement(Expression value)
        {
            Value = value;
        }

        public Expression Value { get; set; }

        public override bool IsEquivalentTo(Syntax other)
            => this == (object)other
            || (other is ReturnStatement rs && ((Value == null && rs.Value == null) || (Value?.IsEquivalentTo(rs?.Value) == true)));

        public override void WriteTo(IndentedTextWriter writer)
        {
            writer.Write("return");
            if (Value != null)
            {
                writer.Write(' ');
                Value.WriteTo(writer);
            }
            writer.WriteLine(';');
        }

        public override bool Reduce()
        {
            if (this.TruReduceReturnValue(Value, out var ov))
            {
                Value = ov;
                return true;
            }

            return false;
        }

        public override Statement Clone()
            => new ReturnStatement(Value);
    }
}