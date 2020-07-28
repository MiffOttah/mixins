using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiffTheFox.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixinTests.Collections
{
    [TestClass]
    public class LockingDictionaryTests
    {
        [TestMethod]
        public void TestDictionary()
        {
            var ld = new LockingDictionary<int, string>
            {
                [1] = "one",
                [2] = "two"
            };
            ld.Add(10, "ten");
            
            _TestDictionary(ld);
        }

        private static void _TestDictionary(IDictionary<int, string> dict)
        {
            Assert.AreEqual("one", dict[1]);
            Assert.AreEqual("two", dict[2]);
            Assert.AreEqual("ten", dict[10]);
            Assert.ThrowsException<KeyNotFoundException>(() => dict[42]);

            CollectionAssert.AreEquivalent(new int[] { 1, 2, 10 }, (ICollection)dict.Keys);
            CollectionAssert.AreEquivalent(new string[] { "one", "two", "ten" }, (ICollection)dict.Values);

            Assert.AreEqual(3, dict.Count);

            Assert.IsTrue(dict.ContainsKey(2));
            Assert.IsFalse(dict.ContainsKey(-1));
            Assert.IsTrue(dict.TryGetValue(10, out string testValue));
            Assert.AreEqual("ten", testValue);
            Assert.IsFalse(dict.TryGetValue(40, out _));
        }

        [TestMethod]
        public void TestLockedGet()
        {
            var ld = new LockingDictionary<int, string>
            {
                [1] = "one",
                [2] = "two"
            };
            ld.Add(10, "ten");
            _TestDictionary(ld.Locked);
        }

        [TestMethod]
        public void TestParentReferenceStability()
        {
            var ld = new LockingDictionary<int, string>
            {
                [1] = "one",
                [2] = "two"
            };
            var locked = ld.Locked;
            ld[2] = "Two!";
            ld.Add(4, "Four?");
            Assert.IsTrue(locked.ContainsKey(4));
            Assert.AreEqual("Two!", ld[2]);
        }

        [TestMethod]
        public void TestLockedSet()
        {
            var ld = new LockingDictionary<int, string>().Locked;
            Assert.ThrowsException<NotSupportedException>(() => ld[0] = "zero");
            Assert.ThrowsException<NotSupportedException>(() => ld.Add(0, "zero"));
            Assert.ThrowsException<NotSupportedException>(() => ld.Clear());
            Assert.ThrowsException<NotSupportedException>(() => ld.Remove(1));
        }
    }
}
