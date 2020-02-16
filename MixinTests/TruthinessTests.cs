using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiffTheFox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixinTests
{
    [TestClass]
    public class TruthinessTests
    {
        [TestMethod]
        public void TestNullTruthiness()
        {
            Assert.IsFalse(Truthiness.IsTruthy(null));
            Assert.IsFalse(Truthiness.IsTruthy((int?)null));
            Assert.IsFalse(Truthiness.IsTruthy(DBNull.Value));
        }

        [TestMethod]
        [DataRow(true, "ABC")]
        [DataRow(true, "1234567890")]
        [DataRow(true, "0")] // this ain't javascript.
        [DataRow(false, "")]
        [DataRow(false, null)]
        public void TestStringTruthiness(bool expected, string value)
        {
            Assert.AreEqual(expected, Truthiness.IsTruthy(value));
        }

        // fixme: this test is currently failing
        // determine which step is failing and address why
        [TestMethod]
        public void TestIntegerTruthiness()
        {
            Assert.IsTrue(Truthiness.IsTruthy((byte)1));
            Assert.IsFalse(Truthiness.IsTruthy((byte)0));

            Assert.IsTrue(Truthiness.IsTruthy((sbyte)1));
            Assert.IsTrue(Truthiness.IsTruthy((sbyte)-1));
            Assert.IsFalse(Truthiness.IsTruthy((sbyte)0));

            Assert.IsTrue(Truthiness.IsTruthy((ushort)1));
            Assert.IsFalse(Truthiness.IsTruthy((ushort)0));

            Assert.IsTrue(Truthiness.IsTruthy((short)1));
            Assert.IsTrue(Truthiness.IsTruthy((short)-1));
            Assert.IsFalse(Truthiness.IsTruthy((short)0));

            Assert.IsTrue(Truthiness.IsTruthy(1U));
            Assert.IsFalse(Truthiness.IsTruthy(1U));

            Assert.IsTrue(Truthiness.IsTruthy(1));
            Assert.IsTrue(Truthiness.IsTruthy(-1));
            Assert.IsFalse(Truthiness.IsTruthy(0));

            Assert.IsTrue(Truthiness.IsTruthy(1UL));
            Assert.IsFalse(Truthiness.IsTruthy(1UL));

            Assert.IsTrue(Truthiness.IsTruthy(1L));
            Assert.IsTrue(Truthiness.IsTruthy(-1L));
            Assert.IsFalse(Truthiness.IsTruthy(0L));
        }

        [TestMethod]
        public void TestSingleTruthiness()
        {
            Assert.IsTrue(Truthiness.IsTruthy(1f));
            Assert.IsTrue(Truthiness.IsTruthy(-1f));
            Assert.IsTrue(Truthiness.IsTruthy(float.PositiveInfinity));
            Assert.IsFalse(Truthiness.IsTruthy(0f));
            Assert.IsFalse(Truthiness.IsTruthy(float.NaN));
        }

        [TestMethod]
        public void TestDoubleTruthiness()
        {
            Assert.IsTrue(Truthiness.IsTruthy(1.0));
            Assert.IsTrue(Truthiness.IsTruthy(-1.0));
            Assert.IsTrue(Truthiness.IsTruthy(double.PositiveInfinity));
            Assert.IsFalse(Truthiness.IsTruthy(0.0));
            Assert.IsFalse(Truthiness.IsTruthy(double.NaN));
        }

        [TestMethod]
        public void TestDecimalTruthiness()
        {
            Assert.IsTrue(Truthiness.IsTruthy(1M));
            Assert.IsTrue(Truthiness.IsTruthy(-1M));
            Assert.IsFalse(Truthiness.IsTruthy(0M));
        }

        [TestMethod]
        public void TestChartruthiness()
        {
            Assert.IsTrue(Truthiness.IsTruthy('A'));
            Assert.IsFalse(Truthiness.IsTruthy('\0'));
        }
    }
}
