namespace NetLib.WPF.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    [ValueConversion(typeof(object), typeof(Brush))]
    public class NotNullToRedConverter : ConvertorBase
    {
        private static readonly Brush red = new SolidColorBrush(Colors.Red);

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? null : red;
        }
    }
}
