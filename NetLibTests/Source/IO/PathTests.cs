using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetLib.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLib.IO.Tests
{
    [TestClass()]
    public class PathTests
    {
        [TestMethod()]
        public void FileExistsFailTest()
        {
            var file = @"\\test\test.txt";
            var res = Path.FileExists(file);

            Assert.AreEqual(false, res);
        }

        [TestMethod()]
        public void FileExistsSuccesTest()
        {
            var file = @"\\picompany.ru\pikp\lib\Новый текстовый документ.txt";
            var res = Path.FileExists(file);

            Assert.AreEqual(true, res);
        }
    }
}