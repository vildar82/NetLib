using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetLib.Tests
{
    [TestClass()]
    public class StringExtTests
    {
        [TestMethod()]
        public void HasCirilicTestFalse()
        {
            var input = "sdrtgs 1 125asdf$%$%^&*~+_*/.*";
            var res = input.HasCirilic();
            Assert.IsFalse(res);
        }

        [TestMethod()]
        public void HasCirilicTestTrue()
        {
            var input = "sdrtрgs 1 125еasdf$%$%^&я**";
            var res = input.HasCirilic();
            Assert.IsTrue(res);
        }
    }
}