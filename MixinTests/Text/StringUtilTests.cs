using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiffTheFox.Text;
using System;

namespace MixinTests.Text
{
    [TestClass]
    public class StringUtilTests
    {
        [TestMethod]
        [DataRow("1,2,3", ',', "1", "2,3")]
        [DataRow("WXYZ", 'Y', "WX", "Z")]
        [DataRow("WXYZ", 'W', "", "XYZ")]
        [DataRow("WXYZ", 'Z', "WXY", "")]
        [DataRow("WXYZ", '!', "WXYZ", "")]
        [DataRow("A//B//C", '/', "A", "/B//C")]
        public void TestPartitionByChar(string source, char needle, string expectedHead, string expectedTail)
        {
            (string head, string tail) = StringUtil.Partition(source, needle);
            Assert.AreEqual(expectedHead, head);
            Assert.AreEqual(expectedTail, tail);
        }

        [TestMethod]
        [DataRow("1,2,3", ",", "1", "2,3")]
        [DataRow("A//B//C", "//", "A", "B//C")]
        [DataRow("WXYZ", "WX", "", "YZ")]
        [DataRow("WXYZ", "YZ", "WX", "")]
        [DataRow("WXYZ", "YZZ", "WXYZ", "")]
        [DataRow("WXYZ", "WZ", "WXYZ", "")]
        public void TestPartitionByString(string source, string needle, string expectedHead, string expectedTail)
        {
            (string head, string tail) = StringUtil.Partition(source, needle);
            Assert.AreEqual(expectedHead, head);
            Assert.AreEqual(expectedTail, tail);
        }

        [TestMethod]
        [DataRow("1,2,3", 1, 3, "1", "3")]
        [DataRow("abcdef", 0, 4, "", "ef")]
        [DataRow("abcdef", 4, 2, "abcd", "")]
        [DataRow("abcdef", 3, 0, "abc", "def")]
        [DataRow("abcdef", -1, 4, "abcdef", "")]
        [DataRow("abcdef", 5, 4, "abcde", "")]
        public void TestPartitionByIndex(string source, int index, int partitionLength, string expectedHead, string expectedTail)
        {
            (string head, string tail) = StringUtil.Partition(source, index, partitionLength);
            Assert.AreEqual(expectedHead, head);
            Assert.AreEqual(expectedTail, tail);
        }

        [TestMethod]
        public void TestPartitionExceptions()
        {
            Assert.ThrowsException<ArgumentNullException>(() => StringUtil.Partition(null, '_'));

            Assert.ThrowsException<ArgumentNullException>(() => StringUtil.Partition(null, "_"));
            Assert.ThrowsException<ArgumentNullException>(() => StringUtil.Partition("abc", null));
            Assert.ThrowsException<ArgumentException>(() => StringUtil.Partition("abc", ""));

            Assert.ThrowsException<ArgumentNullException>(() => StringUtil.Partition(null, 0, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringUtil.Partition("abc", -2, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => StringUtil.Partition("abc", 10, 1));
        }
    }
}
