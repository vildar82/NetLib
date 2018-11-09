﻿using JetBrains.Annotations;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NetLib.WPF.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToCollapseVisibilityConverter : ConvertorBase
    {
        [NotNull]
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool v)
            {
                return v ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Visible;
        }
    }
}
