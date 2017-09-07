using Shipwreck.CSharpModels.Expressions;

namespace Shipwreck.CSharpModels.Statements
{
    public sealed class VariableDeclarator
    {
        public string Identifier { get; set; }

        public Expression Initializer { get; set; }
    }
}