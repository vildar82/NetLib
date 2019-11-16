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
            Path.ClearDir(@"c:\");
            Path.ClearDir(@"c:");
            Path.ClearDir(@"c");
        }
    }
}