namespace NetLib.WPF.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using DynamicData.Annotations;

    [PublicAPI]
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class NegateBoolToCollapseVisibilityConverter : ConvertorBase
    {
        [NotNull]
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return b ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Visible;
        }
    }
}