﻿namespace NetLib.WPF.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using JetBrains.Annotations;

    [PublicAPI]
    [ValueConversion(typeof(bool), typeof(FontWeights))]
    public class BooleanToFontBoldConverter : ConvertorBase
    {
        public override object Convert([NotNull] object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? FontWeights.Bold : FontWeights.Normal;
        }
    }
}