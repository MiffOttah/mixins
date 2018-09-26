using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiffTheFox.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixinTests
{
    [TestClass]
    public class PortionTests
    {
        private const double DELTA = 0.001;

        [TestMethod]
        public void SimplePortionTests()
        {
            Assert.AreEqual(0.0, Portion.Zero.Value, DELTA);
            Assert.AreEqual(1.0, Portion.One.Value, DELTA);

            Assert.AreEqual(0.3, new Portion(0.3).Value, DELTA);
            Assert.AreEqual(0.7, new Portion(0.3).Complement.Value, DELTA);
        }

        [TestMethod]
        public void ClampTests()
        {
            Assert.AreEqual(Portion.One.Value, new Portion(3.14159).Value, DELTA);
            Assert.AreEqual(Portion.Zero.Value, new Portion(-0.3).Value, DELTA);

            Assert.AreEqual(0.0, Portion.Clamp(double.NaN));
            Assert.AreEqual(0.0, Portion.Clamp(-4.2));
            Assert.AreEqual(0.21, Portion.Clamp(0.21));
            Assert.AreEqual(0.74, Portion.Clamp(0.74));
            Assert.AreEqual(1.0, Portion.Clamp(2.71));
            Assert.AreEqual(0.0, Portion.Clamp(double.NegativeInfinity));
            Assert.AreEqual(1.0, Portion.Clamp(double.PositiveInfinity));
        }

        [TestMethod]
        public void ComparisonTests()
        {
            var x = new Portion(0.4);
            var y = new Portion(0.4);
            var z = new Portion(0.8);

            Assert.IsTrue(x.Value == y.Value);
            Assert.IsTrue(x.Value != z.Value);

            Assert.IsTrue(x == y);
            Assert.IsFalse(x == z);
            Assert.IsFalse(y == z);

            Assert.IsFalse(x != y);
            Assert.IsTrue(x != z);
            Assert.IsTrue(y != z);

            Assert.IsTrue(z > x);
            Assert.IsFalse(z < x);
            Assert.IsFalse(x > y);
            Assert.IsTrue(x >= y);
            Assert.IsFalse(x < y);
            Assert.IsTrue(x <= y);

            Assert.AreEqual(0, x.CompareTo(y));
            Assert.IsTrue(x.CompareTo(z) < 0);
            Assert.IsTrue(z.CompareTo(y) > 0);
        }

        [TestMethod]
        public void OperationTests()
        {
            var x = new Portion(0.5);

            Assert.AreEqual(0.7, (x + new Portion(0.2)).Value, DELTA);
            Assert.AreEqual(0.1, (x - new Portion(0.4)).Value, DELTA);
            Assert.AreEqual(0.75, (x * 1.5).Value, DELTA);
            Assert.AreEqual(0.25, (x / 2).Value, DELTA);

            Assert.AreEqual(0.3, (!(new Portion(0.7))).Value, DELTA);

            Assert.AreEqual(4, x.Lerp(8));
            Assert.AreEqual(10, x.Lerp(21));
            Assert.AreEqual(10.5, x.Lerp(21.0), DELTA);
            Assert.AreEqual(0.15, x.Lerp(0.3), DELTA);

            Assert.AreEqual(14, x.Lerp(10, 18));
            Assert.AreEqual(20, x.Lerp(10, 31));
            Assert.AreEqual(20.5, x.Lerp(10.0, 31.0), DELTA);
        }

        [TestMethod]
        public void FractionTests()
        {
            Assert.AreEqual(0.25, Portion.Fraction(4).Value, DELTA);
            Assert.AreEqual(0.4, Portion.Fraction(2, 5).Value, DELTA);
        }

        [TestMethod]
        public void CastTests()
        {
            var p = new Portion(0.75);
            Assert.AreEqual(0.75, (double)p, DELTA);
            Assert.AreEqual(0.75f, (float)p, 0.0001f);
            Assert.AreEqual((byte)191, (byte)p);

            var p2 = new Portion(0.25);
            Assert.AreEqual(0.25, ((Portion)0.25).Value, DELTA);
            Assert.AreEqual(0.25, ((Portion)(byte)64).Value, DELTA);
        }
    }
}
