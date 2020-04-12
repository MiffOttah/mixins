using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiffTheFox;
using MiffTheFox.Collections;
using System;
using System.Collections;
using System.Globalization;
using System.Linq;

namespace MixinTests
{
    [TestClass]
    public class RoundRobinTests
    {
        [TestMethod]
        public void TestCreate()
        {
            var rr = new RoundRobin<int>(10);
            Assert.AreEqual(10, rr.MaximumItems);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-10)]
        public void TestCreateFail(int maximumItems)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new RoundRobin<int>(maximumItems));
        }

        [TestMethod]
        public void TestEmpty()
        {
            var rr = new RoundRobin<int>(10);
            Assert.AreEqual(0, rr.Count);
            foreach (int item in rr)
            {
                Assert.Fail("Enumeration returned values.");
            }
        }

        [TestMethod]
        public void TestPartialFill()
        {
            var rr = new RoundRobin<int>(10);
            for (int i = 0; i < 5; i++)
            {
                rr.Add(i);
            }

            Assert.AreEqual(5, rr.Count);
            CollectionAssert.AreEqual(new int[] { 0, 1, 2, 3, 4 }, rr);
        }

        [TestMethod]
        public void TestOverfill()
        {
            var rr = new RoundRobin<int>(10);
            for (int i = 0; i < 15; i++)
            {
                rr.Add(i);
            }
            Assert.AreEqual(10, rr.Count);
            CollectionAssert.AreEqual(new int[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 }, rr);
        }

        [TestMethod]
        [DataRow("1 2 3 4 5", "1 2 3 4 5")]
        [DataRow("1 2 3 4 5 6 7 8 9 10", "1 2 3 4 5 6 7 8 9 10")]
        [DataRow("6 7 8 9 10 11 12 13 14 15", "1 2 3 4 5 6 7 8 9 10 11 12 13 14 15")]
        public void TestEnumerate(string expected, string data)
        {
            int[] expectedInts = expected.Split(' ').Select(d => int.Parse(d, CultureInfo.InvariantCulture)).ToArray();
            int[] dataInts = data.Split(' ').Select(d => int.Parse(d, CultureInfo.InvariantCulture)).ToArray();

            var rr = new RoundRobin<int>(10);
            foreach (var di in dataInts) rr.Add(di);

            var enumerated = rr.Select(x => x).ToArray(); // use linq to enumerate the roundrobin
            CollectionAssert.AreEqual(expectedInts, enumerated);

            var enumeratedCount = rr.Select(x => x).Count();
            Assert.AreEqual(rr.Count, enumeratedCount);
        }

        [TestMethod]
        public void TestClear()
        {
            var rr = new RoundRobin<int>(10);
            for (int i = 0; i < 5; i++)
            {
                rr.Add(i);
            }
            Assert.AreEqual(5, rr.Count);

            rr.Clear();
            Assert.AreEqual(0, rr.Count);
            CollectionAssert.AreEquivalent(new int[] { }, rr);
        }

        [TestMethod]
        public void TestToArray()
        {
            var rr = new RoundRobin<int>(10);
            for (int i = 0; i < 15; i++)
            {
                rr.Add(i);
            }
            CollectionAssert.AreEqual(new int[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 }, rr.ToArray());
        }

        [TestMethod]
        public void TestCopyTo()
        {
            var rr = new RoundRobin<int>(10);
            for (int i = 0; i < 15; i++)
            {
                rr.Add(i);
            }

            var test = new int[10];
            rr.CopyTo(test, 0);
            CollectionAssert.AreEqual(new int[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 }, test);
        }

        [TestMethod]
        public void TestCopyToMiddle()
        {
            var rr = new RoundRobin<int>(10);
            for (int i = 0; i < 15; i++)
            {
                rr.Add(i);
            }

            var test = new int[12];
            rr.CopyTo(test, 2);
            CollectionAssert.AreEqual(new int[] { 0, 0, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 }, test);
        }

        [TestMethod]
        public void TestCopyToPartial()
        {
            var rr = new RoundRobin<int>(10);
            for (int i = 0; i < 5; i++)
            {
                rr.Add(i);
            }

            var test = new int[10];
            rr.CopyTo(test, 0);
            CollectionAssert.AreEqual(new int[] { 0, 1, 2, 3, 4, 0, 0, 0, 0, 0 }, test);
        }

        [TestMethod]
        public void TestCopyToFail()
        {
            var rr = new RoundRobin<int>(10)
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            };
            var array5 = new int[5];
            var array10 = new int[10];

            Assert.ThrowsException<ArgumentNullException>(() => rr.CopyTo(null, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => rr.CopyTo(array10, -1));
            Assert.ThrowsException<ArgumentException>(() => rr.CopyTo(array5, 0));
            Assert.ThrowsException<ArgumentException>(() => rr.CopyTo(array10, 3));
        }

        [TestMethod]
        public void TestRemoveNotSupported()
        {
            var rr = new RoundRobin<int>(10)
            {
                123,
                456
            };

            Assert.ThrowsException<NotSupportedException>(() => rr.Remove(123));
        }

        [TestMethod]
        public void TestICollectionCopyTo()
        {
            ICollection rr = new RoundRobin<int>(10)
            {
                123,
                456
            };
            int[] res = new int[2];

            rr.CopyTo(res, 0);
            CollectionAssert.AreEqual(new int[] { 123, 456 }, res);
        }

        [TestMethod]
        public void TestICollectionCopyToFail()
        {
            var rr = new RoundRobin<int>(10)
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            };

            var iCollection = (ICollection)rr;
            var array5 = new int[5];
            var array10 = new int[10];
            var invalidArrayType = new string[10];

            Assert.ThrowsException<ArgumentNullException>(() => iCollection.CopyTo(null, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => iCollection.CopyTo(array10, -1));
            Assert.ThrowsException<ArgumentException>(() => iCollection.CopyTo(array5, 0));
            Assert.ThrowsException<ArgumentException>(() => iCollection.CopyTo(array10, 3));
            Assert.ThrowsException<ArgumentException>(() => iCollection.CopyTo(invalidArrayType, 0));
        }
    }
}
