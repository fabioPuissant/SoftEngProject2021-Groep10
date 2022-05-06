using System;
using System.Diagnostics.CodeAnalysis;

namespace VmsApi.Data.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class TaskUserAssignedException : Exception
    {
        public TaskUserAssignedException() : base() { }

        public TaskUserAssignedException(string message) : base(message) { }
    }
}
