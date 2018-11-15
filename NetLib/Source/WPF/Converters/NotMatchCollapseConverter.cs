namespace NetLib.WPF.Converters
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using JetBrains.Annotations;

    /// <summary>
    /// Отрицание - !bool
    /// </summary>
    [PublicAPI]
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class NotMatchCollapseConverter : ConvertorBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            return Equals(value, parameter) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}