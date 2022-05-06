using System;
using System.Diagnostics.CodeAnalysis;

namespace VmsApi.Data.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class IdException: Exception
    {
        public IdException()
        {
            
        }
        public IdException(string message) : base(message)
        {
            
        }
    }
}