namespace NetLib.WPF.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(long), typeof(string))]
    public class Bytes : ConvertorBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((long) value).BytesToString();
        }
    }
}