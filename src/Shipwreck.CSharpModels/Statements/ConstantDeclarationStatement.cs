namespace Shipwreck.CSharpModels.Statements
{
    public sealed partial class ConstantDeclarationStatement : DeclarationStatement
    {
        public override Statement Clone()
        {
            var r = new ConstantDeclarationStatement();
            r.Type = Type;
            if (ShouldSerializeDeclarators())
            {
                foreach (var d in Declarators)
                {
                    r.Declarators.Add(new VariableDeclarator()
                    {
                        Identifier = d.Identifier,
                        Initializer = d.Initializer
                    });
                }
            }

            return r;
        }
    }
}