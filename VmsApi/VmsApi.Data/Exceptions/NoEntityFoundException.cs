using System;
using System.Diagnostics.CodeAnalysis;

namespace VmsApi.Data.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoEntityFoundException : Exception
    {
        public NoEntityFoundException()
        {
            
        }
        public NoEntityFoundException(string msg) : base(msg)
        {
            
        }
    }
}
