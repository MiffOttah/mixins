using Microsoft.VisualStudio.TestTools.UnitTesting;
using MixinTests.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixinTests.Collections
{
    [TestClass]
    public class AnonymousTypeEnumeratorTests
    {
        [TestMethod]
        public void TestEnumeration()
        {
            var anon = new
            {
                Test1 = 1,
                Test2 = 2
            };

            var r = AnonymousTypeEnumerator.Enumerate(anon).ToDictionary(x => x.Key, x => x.Value);
            Assert.AreEqual(2, r.Count);
            CollectionAssert.AreEquivalent(new string[] { "Test1", "Test2" }, r.Keys);
            Assert.AreEqual(1, r["Test1"]);
            Assert.AreEqual(2, r["Test2"]);
        }

        [TestMethod]
        public void TestNullEnumeration()
        {
            var r = AnonymousTypeEnumerator.Enumerate(null).ToArray();
            Assert.AreEqual(0, r.Length);
        }
    }
}
