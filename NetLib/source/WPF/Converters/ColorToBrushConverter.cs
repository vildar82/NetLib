using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace NetLib.WPF.Converters
{
    [ValueConversion(typeof(Color), typeof(SolidColorBrush))]
    [ValueConversion(typeof(System.Drawing.Color), typeof(SolidColorBrush))]
    public class ColorToBrushConverter : ConvertorBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Drawing.Color dc)
            {
                return new SolidColorBrush(Color.FromArgb(dc.A, dc.R, dc.G, dc.B));
            }
            if (value is Color color)
            {
                return new SolidColorBrush(color);
            }
            return null;
        }
    }
}
