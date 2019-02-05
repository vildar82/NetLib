namespace NetLib.WPF.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using JetBrains.Annotations;

    [PublicAPI]
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class NullHiddenVisibilityConverter : ConvertorBase
    {
        [NotNull]
        public override object Convert([CanBeNull] object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Hidden : Visibility.Visible;
        }
    }
}