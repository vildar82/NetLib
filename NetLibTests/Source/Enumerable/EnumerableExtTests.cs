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
	public class EnumerableExtTests
	{
		[TestMethod()]
		public void SelectNullesTest()
		{
			var list = new List<string> { null, null, "gfh", null, "ftgyh", null };
			var selRes = list.SelectNulless(s => s);
			Assert.AreEqual(2, selRes.Count());
		}
	}
}