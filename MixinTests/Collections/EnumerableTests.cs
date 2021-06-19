using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiffTheFox.Collections;
using System;

namespace MixinTests.Collections
{
    [TestClass]
    public class EnumerableTests
    {
        [TestMethod]
        [DataRow("1,2,3", true, "1")]
        [DataRow("X,Y", true, "X")]
        [DataRow("", false, null)]
        public void TestTryFirstWithList(string haystack, bool expectedReturn, string expectedValue)
        {
            var h = haystack.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            bool r = Enumerable.TryFirst(h, out var value);
            Assert.AreEqual(expectedReturn, r);
            if (r) Assert.AreEqual(expectedValue, value);
        }

        [TestMethod]
        [DataRow("AAA,BBB,CCC", "B", true, "BBB")]
        [DataRow("AAA,X,CCC", "B", false, null)]
        [DataRow("test1,test2,test3", "t3", true, "test3")]
        [DataRow("", "X", false, null)]
        public void TestTryFirstWithLinqWhere(string haystack, string needle, bool expectedReturn, string expectedValue)
        {
            var h = haystack.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            bool r = Enumerable.TryFirst(System.Linq.Enumerable.Where(h, x => x.Contains(needle)), out var value);
            Assert.AreEqual(expectedReturn, r);
            if (r) Assert.AreEqual(expectedValue, value);
        }
    }
}
