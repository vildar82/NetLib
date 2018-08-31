using JetBrains.Annotations;
using System;
using System.Globalization;
using System.Windows;
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

        /// <inheritdoc />
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s && targetType.IsEnum)
            {
                foreach (var enumValue in targetType.GetEnumValues())
                {
                    var desc = value.Description();
                    if (desc == s)
                    {
                        return enumValue;
                    }
                }
            }

            return DependencyProperty.UnsetValue;
        }
    }
}
