using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetLib.WPF.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NetLib.WPF.Converters.Tests
{
	[TestClass()]
	public class GridLengthConverterTests
	{
		private GridLengthConverter converter;
		[TestInitialize]
		public void Setup()
		{
			converter = new GridLengthConverter();
		}

		[TestMethod()]
		public void ConvertTest()
		{
			var doubleValueWidth = 100d;
			var gridWidth =(GridLength) converter.Convert(doubleValueWidth, typeof(GridLength), null, CultureInfo.CurrentCulture);
			Assert.AreEqual(100d, gridWidth.Value);
		}

		[TestMethod()]
		public void ConvertBackDoubleTest()
		{
			var gridWidth = new GridLength(100);
			var val = converter.ConvertBack(gridWidth, typeof(double), null, CultureInfo.CurrentCulture);
			Assert.AreEqual(100d, val);
		}
	}
}