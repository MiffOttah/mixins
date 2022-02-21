using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiffTheFox.Text;
using System.Globalization;

namespace MixinTests.Text
{
    [TestClass]
    public class StringBuilderAppendJoinTests
    {
        [TestMethod]
        [DataRow("1,2,3", '_', "1_2_3")]
        [DataRow("abc,def,g,h", ' ', "abc def g h")]
        [DataRow("0,,1", ';', "0;;1")]
        public void TestJoinStringChar(string strings, char seperator, string expected)
        {
            var sb = new StringBuilder();
            sb.AppendJoin(strings?.Split(new char[] { ',' }), seperator);
            Assert.AreEqual(expected, sb.ToString());
        }

        [TestMethod]
        public void TestJoinStringCharEmpty()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(Array.Empty<string>(), ',');
            Assert.AreEqual("", sb.ToString());
        }

        [TestMethod]
        public void TestJoinStringCharNull()
        {
            var sb = new StringBuilder();
            sb.AppendJoin((string[])null, ',');
            Assert.AreEqual("", sb.ToString());
        }

        [TestMethod]
        [DataRow("1,2,3", "_", "1_2_3")]
        [DataRow("abc,def,g,h", "||", "abc||def||g||h")]
        [DataRow("0,,1", "xy", "0xyxy1")]
        [DataRow("h,e,l,l,o", "", "hello")]
        public void TestJoinStringString(string strings, string seperator, string expected)
        {
            var sb = new StringBuilder();
            sb.AppendJoin(strings?.Split(new char[] { ',' }), seperator);
            Assert.AreEqual(expected, sb.ToString());
        }

        [TestMethod]
        public void TestJoinStringStringEmpty()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(Array.Empty<string>(), "||");
            Assert.AreEqual("", sb.ToString());
        }

        [TestMethod]
        public void TestJoinStringStringNull()
        {
            var sb = new StringBuilder();
            sb.AppendJoin((string[])null, "||");
            Assert.AreEqual("", sb.ToString());
        }

        [TestMethod]
        [DataRow("1,2,3", '_', "1_2_3")]
        [DataRow("0,,1", ';', "0;;1")]
        public void TestJoinTChar(string strings, char seperator, string expected)
        {
            var sb = new StringBuilder();
            sb.AppendJoin(strings?.Split(new char[] { ',' })?.Select(x => string.IsNullOrEmpty(x) ? (int?)null : int.Parse(x, NumberStyles.None, CultureInfo.InvariantCulture)), seperator);
            Assert.AreEqual(expected, sb.ToString());
        }

        [TestMethod]
        public void TestJoinTCharEmpty()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(Array.Empty<int>(), ',');
            Assert.AreEqual("", sb.ToString());
        }

        [TestMethod]
        public void TestJoinTCharNull()
        {
            var sb = new StringBuilder();
            sb.AppendJoin((int[])null, ',');
            Assert.AreEqual("", sb.ToString());
        }

        [TestMethod]
        [DataRow("1,2,3", "_", "1_2_3")]
        [DataRow("0,,1", "xy", "0xyxy1")]
        [DataRow("1,2,3,4,5", "", "12345")]
        public void TestJoinTString(string strings, string seperator, string expected)
        {
            var sb = new StringBuilder();
            sb.AppendJoin(strings?.Split(new char[] { ',' })?.Select(x => string.IsNullOrEmpty(x) ? (int?)null : int.Parse(x, NumberStyles.None, CultureInfo.InvariantCulture)), seperator);
            Assert.AreEqual(expected, sb.ToString());
        }

        [TestMethod]
        public void TestJoinTStringEmpty()
        {
            var sb = new StringBuilder();
            sb.AppendJoin(Array.Empty<int>(), "||");
            Assert.AreEqual("", sb.ToString());
        }

        [TestMethod]
        public void TestJoinTStringNull()
        {
            var sb = new StringBuilder();
            sb.AppendJoin((int[])null, "||");
            Assert.AreEqual("", sb.ToString());
        }
    }
}
