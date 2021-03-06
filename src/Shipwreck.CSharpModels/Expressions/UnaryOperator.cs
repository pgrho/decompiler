namespace Shipwreck.CSharpModels.Expressions
{
    public enum UnaryOperator
    {
        UnaryPlus,
        UnaryNegation,
        LogicalNot,
        OnesComplement,

        PreIncrement,
        PreDecrement,
        PostIncrement,
        PostDecrement,

        AddressOf,

        Convert,
        ConvertChecked,
        TypeAs
    }
}