namespace NetLib.WPF.Converters
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public class UniversalValueConverter : ConvertorBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // obtain the conveter for the target type
            var converter = TypeDescriptor.GetConverter(targetType);
            try
            {
                // determine if the supplied value is of a suitable type
                return converter.ConvertFrom(converter.CanConvertFrom(value.GetType()) ? value : value.ToString());
            }
            catch (Exception)
            {
                return value;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}