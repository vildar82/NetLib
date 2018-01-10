using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.Windows;
using GridLengthConverter = NetLib.WPF.Converters.GridLengthConverter;

namespace NetLibTests.Source.WPF.Converters
{
    [TestClass]
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
            var gridWidth = (GridLength)converter.Convert(doubleValueWidth, typeof(GridLength), null, CultureInfo.CurrentCulture);
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