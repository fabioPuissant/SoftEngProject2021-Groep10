using System;
using System.Diagnostics.CodeAnalysis;

namespace VmsApi.Data.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class DatePigGroupException: Exception
    {
        public DatePigGroupException(string message): base(message)
        {
            
        }


    }
}