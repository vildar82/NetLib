namespace NetLib.WPF.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using JetBrains.Annotations;

    [PublicAPI]
    [ValueConversion(typeof(bool), typeof(string))]
    public class BoolToYesNoConverter : ConvertorBase
    {
        [CanBeNull]
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return b ? "Да" : "Нет";
            }

            return null;
        }
    }
}