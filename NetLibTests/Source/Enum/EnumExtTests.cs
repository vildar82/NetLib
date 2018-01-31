using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace NetLib.Tests
{
    [Flags]
    public enum TestEnum
    {
        Default = 0,
        External = 1,
        Polyline = 2,
        Derived = 4,
        Textbox = 8,
        Outermost = 16,
        NotClosed = 32,
        SelfIntersecting = 64,
        TextIsland = 128,
        Duplicate = 256
    }

    [TestClass()]
    public class EnumExtTests
    {
        [TestMethod()]
        public void HasAnyTest1()
        {
            Assert.IsTrue((TestEnum.Default | TestEnum.Duplicate).HasAny(TestEnum.Default));
        }

        [TestMethod()]
        public void HasAnyTest2()
        {
            Assert.IsTrue(TestEnum.External.HasAny(TestEnum.Polyline));
        }
    }
}