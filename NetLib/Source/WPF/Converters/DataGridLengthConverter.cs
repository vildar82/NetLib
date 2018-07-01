using JetBrains.Annotations;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NetLib.WPF.Converters
{
    [PublicAPI]
    [ValueConversion(typeof(double), typeof(DataGridLength))]
    public class DataGridLengthConverter : ConvertorBase
    {
        [NotNull]
        public override object Convert([NotNull] object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d) return new DataGridLength(d);
            throw new ArgumentException("Ожидается double");
        }

        [NotNull]
        public override object ConvertBack([NotNull] object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DataGridLength)value).Value;
        }
    }
}