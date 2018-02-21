using System;

namespace NetLib
{
    public static class DataExt
    {
        public static bool IsEquals(this DateTime date1, DateTime date2)
        {
            return date1.Equals(date2);
        }
    }
}
