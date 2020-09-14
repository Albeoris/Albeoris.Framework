using System;

namespace Albeoris.Framework.System
{
    public class ArgumentEmptyException : ArgumentException
    {
        public ArgumentEmptyException()
            : base("Value cannot be empty.")
        {
        }

        public ArgumentEmptyException(String paramName)
            : base("Value cannot be empty.", paramName)
        {
        }

        public ArgumentEmptyException(String message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ArgumentEmptyException(String paramName, String message)
            : base(message, paramName)
        {
        }
    }
}