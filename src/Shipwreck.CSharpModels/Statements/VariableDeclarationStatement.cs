namespace Shipwreck.CSharpModels.Statements
{
    public sealed partial class VariableDeclarationStatement : DeclarationStatement
    {
        public override Statement Clone()
        {
            var r = new VariableDeclarationStatement();
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