using JetBrains.Annotations;
using System;
using System.Globalization;
using System.Windows.Data;

namespace NetLib.WPF.Converters
{
    [PublicAPI]
    [ValueConversion(typeof(int), typeof(double))]
    [ValueConversion(typeof(double), typeof(double))]
    public class MinToHoursConverter : ConvertorBase
    {
        [NotNull]
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var min = System.Convert.ToInt32(value);
            var hours =(min / 60d).Round(1);
            return hours;
        }
    }
}