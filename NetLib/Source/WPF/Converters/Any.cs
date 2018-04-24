using JetBrains.Annotations;
using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace NetLib.WPF.Converters
{
    /// <summary>
    /// Пустая ли коллекция
    /// </summary>
    [ValueConversion(typeof(object), typeof(bool))]
    public class Empty : ConvertorBase
    {
        [NotNull]
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICollection c) return c.Count == 0;
            return false;
        }
    }
}