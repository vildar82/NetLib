using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NetLib.WPF.Converters
{    
    [ValueConversion(typeof(double), typeof(GridLength))]
	public class GridLengthConverter : ConvertorBase
    {
        public override object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
			if (value is double d) return new GridLength(d);
			throw new ArgumentException($"Ожидается double");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
	        return ((GridLength)value).Value;
        }
    }
}
