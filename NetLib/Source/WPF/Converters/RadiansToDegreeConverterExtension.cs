using JetBrains.Annotations;
using System;
using System.Globalization;
using System.Windows.Data;

namespace NetLib.WPF.Converters
{
    [PublicAPI]
    [ValueConversion(typeof(int), typeof(string))]
    [ValueConversion(typeof(double), typeof(string))]
    [ValueConversion(typeof(double?), typeof(string))]
    public class RadiansToDegreeConverterExtension : ConvertorBase
    {
        [NotNull]
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            double date = System.Convert.ToDouble(value);
            return date.ToDegrees().ToString("N2");
        }

        [NotNull]
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            double date = System.Convert.ToDouble(value);
            return date.ToRadians();
        }
    }
}
