using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using NetLib.WPF.Data;

namespace NetLib.WPF.Converters
{
    [ValueConversion(typeof(byte[]), typeof(ImageSource))]
    public class ImageByteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] array)
            {
                return array.ToImage();
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
