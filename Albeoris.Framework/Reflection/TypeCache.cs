using System;

namespace Albeoris.Framework.Reflection
{
    public static class TypeCache<T>
    {
        public static Type Type { get; } = typeof(T);
        public static TypeCode TypeCode { get; } = Type.GetTypeCode(Type);
    }
}