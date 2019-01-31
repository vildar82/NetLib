namespace NetLib.WPF.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// При навыедении мышки показывать скролбар
    /// ScrollViewer.VerticalScrollBarVisibility="{Binding IsMouseOver, Converter={StaticResource MouseOverToScrollBarVisibility}}">
    /// </summary>
    [ValueConversion(typeof(bool), typeof(ScrollBarVisibility))]
    sealed class MouseOverToScrollbarVisibility : IValueConverter
    {
        private static MouseOverToScrollbarVisibility converter;

        public static MouseOverToScrollbarVisibility Converter => converter ?? (converter = new MouseOverToScrollbarVisibility());

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? ScrollBarVisibility.Auto : ScrollBarVisibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
