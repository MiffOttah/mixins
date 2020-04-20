using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiffTheFox.Collections;

namespace MixinTests.Collections
{
    [TestClass]
    public class EmptyListTests
    {
        // these tests are dead-simple
        // since the EmptyList<T> ignores all input
        // and saves no state

        [TestMethod]
        public void TestCount()
        {
            Assert.AreEqual(0, EmptyList<int>.Shared.Count);
        }

        [TestMethod]
        public void TestIsReadOnly()
        {
            Assert.AreEqual(true, EmptyList<int>.Shared.IsReadOnly);
        }

        [TestMethod]
        public void TestGetByIndex()
        {
            Assert.ThrowsException<IndexOutOfRangeException>(
                () => EmptyList<int>.Shared[0]
            );
        }

        [TestMethod]
        public void TestSetByIndex()
        {
            Assert.ThrowsException<IndexOutOfRangeException>(
                () => EmptyList<int>.Shared[0] = 1
            );
        }

        [TestMethod]
        public void TestContains()
        {
            Assert.IsFalse(EmptyList<int>.Shared.Contains(0));
        }

        [TestMethod]
        public void TestIndexOf()
        {
            Assert.AreEqual(-1, EmptyList<int>.Shared.IndexOf(0));
        }

        [TestMethod]
        public void TestRemove()
        {
            Assert.IsFalse(EmptyList<int>.Shared.Remove(0));
        }

        [TestMethod]
        public void TestAdd()
        {
            Assert.ThrowsException<NotSupportedException>(
                () => EmptyList<int>.Shared.Add(0)
            );
        }

        [TestMethod]
        public void TestInsert()
        {
            Assert.ThrowsException<NotSupportedException>(
                () => EmptyList<int>.Shared.Insert(0, 0)
            );
        }

        [TestMethod]
        public void TestClear()
        {
            Assert.ThrowsException<NotSupportedException>(
                () => EmptyList<int>.Shared.Clear()
            );
        }

        [TestMethod]
        public void TestRemoveAt()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => EmptyList<int>.Shared.RemoveAt(0)
            );
        }

        [TestMethod]
        [DataRow("abc,123,def", 0)]
        [DataRow("w,x,y,z", 2)]
        [DataRow("", 0)]
        [DataRow("abc,123,def", -1)]
        [DataRow("abc,123,def", 100)]
        public void TestCopyTo(string testData, int index)
        {
            var expected = testData.Split(',');
            var actual = testData.Split(',');
            EmptyList<string>.Shared.CopyTo(actual, index);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestEnumerate()
        {
            foreach (int item in EmptyList<int>.Shared)
            {
                Assert.Fail();
            }
        }
    }
}
