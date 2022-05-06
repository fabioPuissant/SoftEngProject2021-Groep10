using System;
using System.Diagnostics.CodeAnalysis;

namespace VmsApi.Data.Utils
{
    [ExcludeFromCodeCoverage]
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string message) : base(message)
        {

        }
    }
}