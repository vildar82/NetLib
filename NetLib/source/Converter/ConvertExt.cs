using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLib
{
    public static class ConvertExt
    {
        public static T GetValue<T>(this object value)
        {
            var culture = CultureInfo.InvariantCulture;
            if (value is string && typeof(T) == typeof(double))
            {
                value = ((string)value).Replace(",", ".");
                culture = new CultureInfo("en-US");
            }
            return (T)Convert.ChangeType(value, typeof(T), culture);
        }
    }
}
