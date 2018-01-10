using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetLib.WPF.Behaviors;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NetLibTests.Source.WPF.Behaviors.DataGrid
{
    [TestClass]
    public class AlphaNumSorterTests
    {
        [Ignore]
        [TestMethod]
        public void CompareTest()
        {
            var list = new ArrayList { "10", "1", "32", "2", "58", "100" };
            var sorter = new AlphaNumSorter();

            list.Sort(sorter);

            var actualList = new List<string> { "1", "2", "10", "32", "58", "100" };
            var listList = list.Cast<string>();
            Assert.AreEqual(actualList.SequenceEqual(listList), true);
        }
    }
}