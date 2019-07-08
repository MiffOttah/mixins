using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiffTheFox;
using MiffTheFox.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixinTests
{
    [TestClass]
    public class DoubleDictionaryTests
    {
        [TestMethod]
        public void SimpleDictionaryTest()
        {
            var d = new DoubleDictionary<int, string>
            {
                { 1, "Hello" },
                { 2, "World" }
            };

            Assert.AreEqual(2, d.Count);
            Assert.AreEqual("World", d[2]);
            Assert.ThrowsException<KeyNotFoundException>(() => d[42]);
        }

        [TestMethod]
        public void SetByIndexerTest()
        {
            var d = new DoubleDictionary<int, string>();
            d[1] = "one";
            d[10] = "ten";
            d[100] = "hundred";

            Assert.AreEqual(3, d.Count);
            Assert.AreEqual("ten", d[10]);

            d[100] = "one hundred";
            Assert.AreEqual("one hundred", d[100]);

            Assert.ThrowsException<InvalidOperationException>(() => d[2] = "one");
            Assert.ThrowsException<InvalidOperationException>(() => d[10] = "one");

            Assert.AreEqual(3, d.Count);
            Assert.AreEqual("ten", d[10]);
        }

        [TestMethod]
        public void GetKeysAndValuesTest()
        {
            var d = new DoubleDictionary<int, string>
            {
                { 1, "Hello" },
                { 2, "World" }
            };

            CollectionAssert.AreEquivalent(new int[] { 1, 2 }, d.Keys.ToArray());
            CollectionAssert.AreEquivalent(new string[] { "Hello", "World" }, d.Values.ToArray());
        }

        [TestMethod]
        public void AddTest()
        {
            var d = new DoubleDictionary<int, string>
            {
                { 1, "Hello" },
                { 2, "World" }
            };

            Assert.AreEqual(2, d.Count);
            d.Add(3, "Goodbye");
            Assert.AreEqual(3, d.Count);

            Assert.ThrowsException<InvalidOperationException>(() => d.Add(1, "foo"));
            Assert.ThrowsException<InvalidOperationException>(() => d.Add(4, "World"));
        }

        [TestMethod]
        public void ContainsTest()
        {
            var d = new DoubleDictionary<int, string>
            {
                { 1, "Hello" },
                { 2, "World" }
            };

            Assert.IsTrue(d.ContainsKey(1));
            Assert.IsFalse(d.ContainsKey(10));
            Assert.IsTrue(d.ContainsValue("Hello"));
            Assert.IsFalse(d.ContainsValue("Goodbye"));
        }

        [TestMethod]
        public void EnumerationTest()
        {
            var d = new DoubleDictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" },
                { 3, "three" },
                { 10, "ten" }
            };
            var de = (IEnumerable<KeyValuePair<int, string>>)d;

            Assert.AreEqual(4, de.Count());
            CollectionAssert.AreEquivalent(new int[] { 1, 2, 3, 10 }, d.Select(e => e.Key).ToArray());
        }
    }
}
