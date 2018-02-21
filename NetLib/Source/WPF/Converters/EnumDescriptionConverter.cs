using JetBrains.Annotations;
using System;
using System.Globalization;
using System.Windows.Data;

namespace NetLib.WPF.Converters
{
    [ValueConversion(typeof(Enum), typeof(string))]
    public class EnumDescriptionConverter : ConvertorBase
    {
        [CanBeNull]
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Description();
        }
    }
}
