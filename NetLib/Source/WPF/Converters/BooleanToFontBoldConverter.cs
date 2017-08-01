﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NetLib.WPF.Converters
{
	[ValueConversion(typeof(bool), typeof(FontWeights))]
	public class BooleanToFontBoldConverter : ConvertorBase
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)value ? FontWeights.Bold : FontWeights.Normal;
		}
	}
}