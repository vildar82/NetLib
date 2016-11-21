using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NetLib.WPF.Converters
{
    [ValueConversion(typeof(Enum), typeof(string))]
    public class EnumDescriptionConverter : ConvertorBase
    {
        public override object Convert (object value, Type targetType, object parameter, CultureInfo culture)
        {            
            return value.Description();
        }
    }
}
