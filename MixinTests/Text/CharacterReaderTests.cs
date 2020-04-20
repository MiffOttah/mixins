using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiffTheFox.Text;

namespace MixinTests.Text
{
    [TestClass]
    public class CharacterReaderTests
    {
        [TestMethod]
        [DataRow("")]
        [DataRow("abcd")]
        public void TestCreation(string text)
        {
            var reader = new CharacterReader(text);
            Assert.AreEqual(text, reader.Text);
            Assert.AreEqual(text.Length, reader.Length);
        }

        [TestMethod]
        public void TestFailedCreation()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new CharacterReader(null));
        }

        [TestMethod]
        public void TestSkipReset()
        {
            var reader = new CharacterReader("abcd");
            reader.Skip(2);
            Assert.AreEqual('c', reader.Read());
            reader.Reset();
            Assert.AreEqual('a', reader.Read());
        }

        [TestMethod]
        public void TestSkipFail()
        {
            var reader = new CharacterReader("abcd");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => reader.Skip(-1));
        }

        [TestMethod]
        [DataRow('1', "1234")]
        [DataRow('a', "a")]
        [DataRow('\0', "\0\0")]
        [DataRow(null, "")]
        public void TestRead(char? expected, string text)
        {
            var reader = new CharacterReader(text);
            Assert.AreEqual(expected, reader.Read());
        }

        [TestMethod]
        [DataRow(true, '1', "1234")]
        [DataRow(true, 'a', "a")]
        [DataRow(true, '\0', "\0\0")]
        [DataRow(false, '\0', "")]
        public void TestTryRead(bool expectedReturn, char expectedCharacter, string text)
        {
            var reader = new CharacterReader(text);
            Assert.AreEqual(expectedReturn, reader.TryRead(out char c));
            Assert.AreEqual(expectedCharacter, c);
        }

        [TestMethod]
        [DataRow(true, "ab", 'c', 2, "abcd")]
        [DataRow(true, "abc", 'd', 3, "abcd")]
        [DataRow(true, "abcd", null, 4, "abcd")]
        [DataRow(false, "abcd", null, 5, "abcd")]
        [DataRow(false, null, null, 2, "")] // position will start at end of string for empty string input
        public void TestTryReadString(bool expectedReturn, string expectedString, char? expectedNextRead, int count, string text)
        {
            var reader = new CharacterReader(text);
            Assert.AreEqual(expectedReturn, reader.TryReadString(count, out string str));
            Assert.AreEqual(expectedString, str);
            Assert.AreEqual(expectedNextRead, reader.Read(), "Read character following string");
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void TestTryReadStringFail(int length)
        {
            var reader = new CharacterReader("abcd");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => reader.TryReadString(length, out _));
        }

        [TestMethod]
        [DataRow(true, "abc", "def", "abc,def", ',', 0)]
        [DataRow(true, "abc", "def,ghi", "abc,def,ghi", ',', 0)]
        [DataRow(true, "def", "ghi", "abc,def,ghi", ',', 4)]
        [DataRow(false, "ghi", null, "abc,def,ghi", ',', 8)]
        [DataRow(false, "abcdefghi", null, "abcdefghi", ',', 0)]
        [DataRow(true, "", "def,ghi", ",def,ghi", ',', 0)]
        public void TestTryReadTo(bool expectedReturn, string expectedString, string expectedRest, string text, char readTo, int skip)
        {
            var reader = new CharacterReader(text);
            reader.Skip(skip);

            Assert.AreEqual(expectedReturn, reader.TryReadTo(readTo, out string str));
            Assert.AreEqual(expectedString, str);

            reader.TryReadString(1000, out string rest);
            Assert.AreEqual(expectedRest, rest);
        }
    }
}
