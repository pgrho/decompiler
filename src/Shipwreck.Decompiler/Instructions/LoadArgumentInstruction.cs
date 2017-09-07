using Shipwreck.CSharpModels.Expressions;

namespace Shipwreck.Decompiler.Instructions
{
    public sealed class LoadArgumentInstruction : LoadIndexInstruction
    {
        public LoadArgumentInstruction(int index, byte argumentSize = 4)
            : base(index)
        {
            ArgumentSize = argumentSize;
        }

        public byte ArgumentSize { get; }

        public ushort OpCode
        {
            get
            {
                switch (ArgumentSize)
                {
                    case 0:
                        return (ushort)(0x02 + Index);

                    case 1:
                        return 0x0e;
                }

                return 0xfe09;
            }
        }

        #region Macro

        private static LoadArgumentInstruction[] _Macros;

        internal static LoadArgumentInstruction GetMacro(int index)
        {
            var a = _Macros ?? (_Macros = new LoadArgumentInstruction[4]);
            return a[index] ?? (a[index] = new LoadArgumentInstruction(index, 0));
        }

        #endregion Macro

        internal override bool TryCreateExpression(DecompilationContext context, ref int index, out Expression expression)
        {
            if (context.Method.IsStatic)
            {
                expression = context.GetParameter(Index);
            }
            else if (Index == 0)
            {
                expression = context.This;
            }
            else
            {
                expression = context.GetParameter(Index - 1);
            }

            return true;
        }

        public override bool IsEqualTo(Instruction other)
            => this == other
            || (other is LoadArgumentInstruction li
                && Index == li.Index
                && ArgumentSize == li.ArgumentSize);

        public override string ToString()
        {
            switch (ArgumentSize)
            {
                case 0:
                    return $"ldarg.{Index}";

                case 1:
                    return $"ldarg.s.{Index}";
            }
            return $"ldarg {Index}";
        }
    }
}