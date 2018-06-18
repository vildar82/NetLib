using JetBrains.Annotations;
using System;
using System.Globalization;
using System.Windows.Data;

namespace NetLib.WPF.Converters
{
    /// <summary>
    /// True - если объект null
    /// </summary>
    [PublicAPI]
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsNullConverter : ConvertorBase
    {
        public override object Convert([CanBeNull] object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }
    }

    /// <summary>
    /// True - если объект null
    /// </summary>
    [PublicAPI]
    [ValueConversion(typeof(object), typeof(bool))]
    public class IsNull : ConvertorBase
    {
        public override object Convert([CanBeNull] object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }
    }
}