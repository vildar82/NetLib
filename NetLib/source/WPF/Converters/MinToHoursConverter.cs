using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace NetLib.WPF.Converters
{
    [ValueConversion(typeof(int), typeof(double))]
    [ValueConversion(typeof(double), typeof(double))]
    public class MinToHoursConverter : ConvertorBase
    {
        public override object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            var min = System.Convert.ToInt32(value);
            var hours = Math.Round(min / 60d, 1);
            return hours;
        }
    }
}
