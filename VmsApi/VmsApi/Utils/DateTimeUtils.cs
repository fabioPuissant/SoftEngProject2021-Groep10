using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;


namespace VmsApi.Utils
{
    [ExcludeFromCodeCoverage]
    public static class DateTimeUtils
    {
        public static DateTime GenerateDateTimeFromString(string dateTimeString)
        {
            DateTime.TryParseExact(dateTimeString, "dd/MM/yyyy", null, DateTimeStyles.None, out var parsedDate);

            return parsedDate;
            
        }
    }
}
