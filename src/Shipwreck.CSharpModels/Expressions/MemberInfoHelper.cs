using System;
using System.Reflection;

namespace Shipwreck.CSharpModels.Expressions
{
    internal static class MemberInfoHelper
    {
        public static bool IsEqualTo(this MemberInfo member, MemberInfo other)
            => member == other;
    }
}