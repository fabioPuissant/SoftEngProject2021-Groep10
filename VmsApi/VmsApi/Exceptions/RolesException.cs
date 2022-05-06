using System;
using System.Diagnostics.CodeAnalysis;

namespace VmsApi.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class RolesException : Exception
    {
        public RolesException(string message) : base (message)
        {
            
        }
    }
}