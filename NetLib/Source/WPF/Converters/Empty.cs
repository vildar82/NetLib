using JetBrains.Annotations;
using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace NetLib.WPF.Converters
{
    /// <summary>
    /// Содержит ли колекция хотя бы один элемент
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public class Any : ConvertorBase
    {
        [NotNull]
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICollection c) return c.Count > 0;
            return false;
        }
    }
}