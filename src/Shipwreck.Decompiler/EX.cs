using System;
using System.Runtime;

namespace Shipwreck.Decompiler
{
    internal static class EX
    {
        [TargetedPatchingOptOut(null)]
        public static void ArgumentIsNotNull<T>(this T value, string name)
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}