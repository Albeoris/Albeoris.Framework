using System;

namespace Albeoris.Framework.Exceptions
{
    public static class Errors
    {
        public static NotSupportedException NotSupported<T>(T value) where T : Enum
        {
            Type enumType = typeof(T);
            
            return Enum.IsDefined(enumType, value)
                ? new NotSupportedException($"{enumType.FullName}.{value} is not supported.")
                : new NotSupportedException($"The value {value} is not defined for an enum of type {enumType.FullName}.");
        }
    }
}