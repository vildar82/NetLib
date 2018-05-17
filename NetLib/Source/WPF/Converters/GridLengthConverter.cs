using JetBrains.Annotations;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NetLib.WPF.Converters
{
    [PublicAPI]
    [ValueConversion(typeof(double), typeof(GridLength))]
    public class GridLengthConverter : ConvertorBase
    {
        [NotNull]
        public override object Convert([NotNull] object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d) return new GridLength(d);
            throw new ArgumentException("Ожидается double");
        }

        [NotNull]
        public override object ConvertBack([NotNull] object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((GridLength)value).Value;
        }
    }
}