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
	/// <summary>
    /// True - если объект null
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsNullConverter : ConvertorBase
    {
        public override object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }
    }
}
