using System;
using System.Globalization;

namespace PP.Core.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime GetMinDateTime()
        {
            return DateTime.Parse("1900-01-01");
        }

        public static DateTime GetMyLocalTime(this DateTime utcDate)
        {
            return utcDate.ToLocalTime();

        }

        public static DateTime GetMyUtcTime(this DateTime localDate)
        {
            return localDate.ToUniversalTime();
        }
    }
}
