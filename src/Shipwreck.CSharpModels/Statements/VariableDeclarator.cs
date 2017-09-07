using Shipwreck.Decompiler.Expressions;

namespace Shipwreck.Decompiler.Statements
{
    public sealed class VariableDeclarator
    {
        public string Identifier { get; set; }

        public Expression Initializer { get; set; }
    }
}