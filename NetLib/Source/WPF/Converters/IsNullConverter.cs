namespace NetLib.WPF.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// True - если объект null
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsNullConverter : ConvertorBase
    {
        public override object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }
    }

    /// <summary>
    /// True - если объект null
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsNull : ConvertorBase
    {
        public override object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }
    }
}