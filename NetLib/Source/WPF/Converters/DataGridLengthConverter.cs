namespace NetLib.WPF.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    [ValueConversion(typeof(double), typeof(DataGridLength))]
    public class DataGridLengthConverter : ConvertorBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
                return new DataGridLength(d);
            throw new ArgumentException("Ожидается double");
        }

        public override object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DataGridLength)value).Value;
        }
    }
}