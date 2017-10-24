using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NetLib.WPF.Converters
{
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
