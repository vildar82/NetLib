using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetLib.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLib.IO.Tests
{
    [TestClass()]
    public class PathTests
    {
        [TestMethod()]
        public void AuthorTest()
        {
            var file = @"W:\C3D_Projects\0181_Ярославль_Волга Парк\2-Рабочие Чертежи\ГП\0181-Р-ПЛ-ГП_КЗ_2019-03-21.dwg";            
            var actual = Path.Author(file);
            var expected = "main\\soverchenkodd";
            Assert.AreEqual(expected, actual.ToLower());
        }
    }
}