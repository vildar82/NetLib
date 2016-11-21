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
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToHidingVisibilityConverter : ConvertorBase
    {
        public override object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return Visibility.Hidden;            
            if ((bool)value)
            {
                return Visibility.Visible;
            }
            return Visibility.Hidden;
        }
    }
}
