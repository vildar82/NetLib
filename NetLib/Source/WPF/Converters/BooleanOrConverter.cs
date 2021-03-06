﻿namespace NetLib.WPF.Converters
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;
    using System.Windows.Markup;
    using JetBrains.Annotations;

    [PublicAPI]
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public class BooleanOrConverter : MarkupExtension, IMultiValueConverter
    {
        [NotNull]
        public object Convert([NotNull] object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Any(value => (bool)value);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        [NotNull]
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}