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
            Assert.IsFalse(Truthiness.IsTruthy<object>(null));
            Assert.IsFalse(Truthiness.IsTruthy((int?)null));
            Assert.IsFalse(Truthiness.IsTruthy(DBNull.Value));
        }

        [TestMethod]
        [DataRow(true, true, DisplayName = "boolean true")]
        [DataRow(false, false, DisplayName = "boolean false")]
        public void TestBooleanTruthiness(bool expected, bool test)
        {
            Assert.AreEqual(expected, test.IsTruthy());
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

        [TestMethod]
        [DataRow(true, (byte)1, DisplayName = "byte 1")]
        [DataRow(false, (byte)0, DisplayName = "byte 0")]
        [DataRow(true, (sbyte)1, DisplayName = "sbyte 1")]
        [DataRow(true, (sbyte)-1, DisplayName = "sbyte -1")]
        [DataRow(false, (sbyte)0, DisplayName = "sbyte 0")]
        [DataRow(true, (ushort)1, DisplayName = "ushort 1")]
        [DataRow(false, (ushort)0, DisplayName = "ushort 0")]
        [DataRow(true, (short)1, DisplayName = "short 1")]
        [DataRow(true, (short)-1, DisplayName = "short -1")]
        [DataRow(false, (short)0, DisplayName = "short 0")]
        [DataRow(true, 1U, DisplayName = "uint 1")]
        [DataRow(false, 0U, DisplayName = "uint 0")]
        [DataRow(true, 1, DisplayName = "int 1")]
        [DataRow(true, -1, DisplayName = "int -1")]
        [DataRow(false, 0, DisplayName = "int 0")]
        [DataRow(true, 1UL, DisplayName = "ulong 1")]
        [DataRow(false, 0UL, DisplayName = "ulong 0")]
        [DataRow(true, 1L, DisplayName = "long 1")]
        [DataRow(true, -1L, DisplayName = "long -1")]
        [DataRow(false, 0L, DisplayName = "long 0")]
        public void TestIntegerTruthiness(bool expected, object data)
        {
            Assert.AreEqual(expected, data.IsTruthy());
        }

        [TestMethod]
        [DataRow(true, 1f, DisplayName = "single 1")]
        [DataRow(true, -1f, DisplayName = "single -1")]
        [DataRow(false, 0f, DisplayName = "single 0")]
        [DataRow(false, float.NaN, DisplayName = "single NaN")]
        [DataRow(true, float.PositiveInfinity, DisplayName = "single infinity")]
        [DataRow(true, 1.0, DisplayName = "double 1")]
        [DataRow(true, -1.0, DisplayName = "double -1")]
        [DataRow(false, 0.0, DisplayName = "double 0")]
        [DataRow(false, double.NaN, DisplayName = "double NaN")]
        [DataRow(true, double.PositiveInfinity, DisplayName = "double infinity")]
        public void TestFloatingPointTruthiness(bool expected, object data)
        {
            Assert.AreEqual(expected, data.IsTruthy());
        }

        [TestMethod]
        public void TestDecimalTruthiness()
        {
            Assert.IsTrue(Truthiness.IsTruthy(1M));
            Assert.IsTrue(Truthiness.IsTruthy(-1M));
            Assert.IsFalse(Truthiness.IsTruthy(0M));
        }

        [TestMethod]
        public void TestCharTruthiness()
        {
            Assert.IsTrue(Truthiness.IsTruthy('A'));
            Assert.IsFalse(Truthiness.IsTruthy('\0'));
        }

        [TestMethod]
        [DataRow(true, ByteEnum.Yes, DisplayName = "byte true")]
        [DataRow(false, ByteEnum.No, DisplayName = "byte false")]
        [DataRow(true, IntEnum.Yes, DisplayName = "int true")]
        [DataRow(false, IntEnum.No, DisplayName = "int false")]
        [DataRow(true, ULongEnum.Yes, DisplayName = "ulong true")]
        [DataRow(false, ULongEnum.No, DisplayName = "ulong false")]
        public void TestEnumTruthiness(bool expected, object test)
        {
            Assert.AreEqual(expected, test.IsTruthy());
        }

        [TestMethod]
        public void TestCollectionTruthiness()
        {
            var arrayTrue = new int[] { 1, 2, 3 };
            var arrayFalse = new int[0];
            Assert.IsTrue(arrayTrue.IsTruthy());
            Assert.IsFalse(arrayFalse.IsTruthy());
        }

        [TestMethod]
        public void TestEnumerableTruthiness()
        {
            IEnumerable<int> enumerate(int count)
            {
                while (count-- > 0)
                {
                    yield return count;
                }
            }

            Assert.IsTrue(enumerate(3).IsTruthy());
            Assert.IsFalse(enumerate(0).IsTruthy());
        }

        [TestMethod]
        public void TestUnknownTypeTruthiness()
        {
            // types unknown to Truthiness.cs are
            // considered to be truthy at all times
            // (unless null)
            Assert.IsTrue(Truthiness.IsTruthy(DateTime.Now));
            Assert.IsTrue(Truthiness.IsTruthy(Encoding.UTF8));
            Assert.IsTrue(Truthiness.IsTruthy(System.Threading.Thread.CurrentThread));
        }

        // enums to test enum truthiness
        enum ByteEnum : byte { No = 0, Yes = 1 }
        enum IntEnum : int { No = 0, Yes = 1 }
        enum ULongEnum : ulong { No = 0, Yes = 1 }
    }
}
