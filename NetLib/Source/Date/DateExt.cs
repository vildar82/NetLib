using System;

namespace NetLib.Date
{
    public static class DateExt
    {
        public static bool IsEquals(this DateTime date1, DateTime date2)
        {
            return date1.Equals(date2);
        }
    }
}
