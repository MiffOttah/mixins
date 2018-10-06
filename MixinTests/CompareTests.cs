using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiffTheFox;
using System;
using System.Linq;

namespace MixinTests
{
    [TestClass]
    public class CompareTests
    {
        struct TestComparable : IComparable<TestComparable>
        {
            public int Value;

            public int CompareTo(TestComparable other) => Value.CompareTo(other.Value);
        }

        [TestMethod]
        public void ComparableComparerTest()
        {
            var c = new ComparableComparer<TestComparable>();

            var test0 = new TestComparable { Value = 0 };
            var test1 = new TestComparable { Value = 100 };
            var test2 = new TestComparable { Value = 200 };
            var test3 = new TestComparable { Value = 1000 };

            Assert.AreEqual(-1, c.Compare(test0, test1));
            Assert.AreEqual(-1, c.Compare(test0, test2));
            Assert.AreEqual(1, c.Compare(test3, test2));
        }

        [TestMethod]
        public void EnumerableOrderedTest()
        {
            var test = new TestComparable[]
            {
                new TestComparable { Value = 0 },
                new TestComparable { Value = -20 },
                new TestComparable { Value = 15 },
                new TestComparable { Value = 490 },
                new TestComparable { Value = 100 },
                new TestComparable { Value = -500 }
            };

            CollectionAssert.AreEqual(
                new int[] { -500, -20, 0, 15, 100, 490 },
                test.Ordered().Select(x => x.Value).ToArray()
            );

            var c = new ComparableComparer<TestComparable>();

            CollectionAssert.AreEqual(
                new int[] { 490, 100, 15, 0, -20, -500 },
                test.Ordered(c.Reversed()).Select(x => x.Value).ToArray()
            );

        }

        [TestMethod]
        public void MapComparerTest()
        {
            var c = new MapComparer<string, string>(s => s.ToUpperInvariant());

            Assert.AreEqual(-1, c.Compare("ABC", "DEF"));

            Assert.AreEqual(1, c.Compare("DEF", "ABC"));
            Assert.AreEqual(1, c.Compare("def", "ABC"));
            Assert.AreEqual(1, c.Compare("DEF", "abc"));
        }
    }
}
