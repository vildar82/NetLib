using System;
using System.Globalization;
using System.Windows.Data;
using JetBrains.Annotations;

namespace NetLib.WPF.Converters
{
    [PublicAPI]
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BooleanToOppositeBooleanConverter : ConvertorBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return !b;
            }
            return false;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}