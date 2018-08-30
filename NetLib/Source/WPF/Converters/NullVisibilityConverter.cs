using JetBrains.Annotations;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NetLib.WPF.Converters
{
    [PublicAPI]
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class NullVisibilityConverter : ConvertorBase
    {
        [NotNull]
        public override object Convert([CanBeNull] object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}