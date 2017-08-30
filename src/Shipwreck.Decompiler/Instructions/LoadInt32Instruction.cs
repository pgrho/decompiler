namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadInt32Instruction : Instruction
    {
        public LoadInt32Instruction(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}