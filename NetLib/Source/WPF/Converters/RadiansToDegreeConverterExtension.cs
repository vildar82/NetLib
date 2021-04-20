namespace NetLib.WPF.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(int), typeof(string))]
    [ValueConversion(typeof(double), typeof(string))]
    [ValueConversion(typeof(double?), typeof(string))]
    public class RadiansToDegreeConverterExtension : ConvertorBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            double date = System.Convert.ToDouble(value);
            return date.ToDegrees().ToString("N2");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            double date = System.Convert.ToDouble(value);
            return date.ToRadians();
        }
    }
}
