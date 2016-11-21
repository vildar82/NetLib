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
    [ValueConversion(typeof(int), typeof(string))]
    [ValueConversion(typeof(double), typeof(string))]
    public class RadiansToDegreeConverterExtension : ConvertorBase
    {        
        public override object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            double date = System.Convert.ToDouble(value);
            return date.ToDegrees().ToString("N2");
        }
        public override object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
        {
            double date = System.Convert.ToDouble(value);
            return date.ToRadians();
        }       
    }
}
