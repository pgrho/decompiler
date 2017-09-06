namespace Shipwreck.Decompiler.ILDecompilerTests
{
    internal struct Zero
    {
        #region Unary

        public static Zero operator +(Zero l)
            => l;

        public static Zero operator -(Zero l)
            => l;

        public static Zero operator !(Zero l)
            => l;

        public static Zero operator ~(Zero l)
            => l;

        public static Zero operator ++(Zero l)
            => l;

        public static Zero operator --(Zero l)
            => l;

        public static bool operator true(Zero l)
            => false;

        public static bool operator false(Zero l)
            => true;

        #endregion Unary

        #region Comparison

        public static bool operator ==(Zero l, Zero r)
            => true;

        public static bool operator !=(Zero l, Zero r)
            => false;

        public static bool operator <(Zero l, Zero r)
            => false;

        public static bool operator >(Zero l, Zero r)
            => false;

        public static bool operator <=(Zero l, Zero r)
            => false;

        public static bool operator >=(Zero l, Zero r)
            => false;

        #endregion Comparison

        #region Arithmetic

        public static Zero operator +(Zero l, Zero r)
            => default(Zero);

        public static Zero operator -(Zero l, Zero r)
            => default(Zero);

        public static Zero operator *(Zero l, Zero r)
            => default(Zero);

        public static Zero operator /(Zero l, Zero r)
            => default(Zero);

        public static Zero operator %(Zero l, Zero r)
            => default(Zero);

        #endregion Arithmetic

        #region bitwise

        public static Zero operator &(Zero l, Zero r)
            => default(Zero);

        public static Zero operator |(Zero l, Zero r)
            => default(Zero);

        public static Zero operator ^(Zero l, Zero r)
            => default(Zero);

        public static Zero operator <<(Zero l, int r)
            => default(Zero);

        public static Zero operator >>(Zero l, int r)
            => default(Zero);

        #endregion bitwise

        public override bool Equals(object obj)
            => obj is Zero;

        public override int GetHashCode()
            => 0;

        public override string ToString()
            => "{zero}";
    }
}