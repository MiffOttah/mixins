using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiffTheFox;
using MiffTheFox.Collections;

namespace MixinTests.Collections
{
    [TestClass]
    public class CacheTests
    {
        [TestMethod]
        public void CreateCacheTests()
        {
            var cache = new Cache<int, string>();
            Assert.AreEqual(0, cache.Count);

            cache.Add(1, "one");
            cache.Add(2, "two");

            Assert.AreEqual(2, cache.Count);
            CollectionAssert.AreEquivalent(new string[] { "one", "two" }, cache.ToArray());

            cache.Add(1, "One");
            Assert.AreEqual(2, cache.Count);
            CollectionAssert.AreEquivalent(new string[] { "One", "two" }, cache.ToArray());
        }

        [TestMethod]
        public void AccessTest()
        {
            var cache = new Cache<int, string>();
            cache.Add(1, "one");
            cache.Add(2, "two");

            Assert.IsTrue(cache.GetItem(2, out string value));
            Assert.AreEqual("two", value);
            Assert.IsFalse(cache.GetItem(3, out value));

            Assert.AreEqual("two", cache.GetItem(2, () => { Assert.Fail(); return null; }));
            Assert.AreEqual("four", cache.GetItem(4, () => "four"));
            Assert.AreEqual("four", cache.GetItem(4, () => { Assert.Fail(); return null; }));
        }

        [TestMethod]
        public void ExpireTest()
        {
            var cache = new Cache<int, string>(TimeSpan.FromSeconds(0.5));

            cache.Add(1, "one");
            Assert.AreEqual(1, cache.Count);
            Thread.Sleep(1000);
            Assert.AreEqual(0, cache.Count);
        }

        [TestMethod]
        public void CapacityTest()
        {
            var cache = new Cache<int, string>(5);

            for (int i = 0; i < 10; i++)
            {
                cache.Add(i, i.ToString());
                Thread.Sleep(10);
            }

            Assert.AreEqual(5, cache.Count);
            Assert.IsFalse(cache.GetItem(1, out string value));
            Assert.IsTrue(cache.GetItem(8, out value));
        }
    }
}
