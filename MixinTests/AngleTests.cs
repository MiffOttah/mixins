using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiffTheFox.Geometry;
using AngleUnit = MiffTheFox.Geometry.Angle.AngleUnit;
using System;

namespace MixinTests
{
    [TestClass]
    public class AngleTests
    {
        private const double DELTA = 0.001;

        [TestMethod]
        public void TestCreation()
        {
            var quarterAngles = new Angle[]
            {
                new Angle(0.25, AngleUnit.Turns),
                new Angle(90, AngleUnit.Degrees),
                new Angle(Math.PI / 2, AngleUnit.Radians),
                new Angle(0.5, AngleUnit.PiRadians),
                new Angle(100, AngleUnit.Gradians),
                new Angle(25, AngleUnit.Percent)
            };

            for (int i = 0; i < quarterAngles.Length; i++)
            {
                for (int j = 0; j < quarterAngles.Length; j++)
                {
                    Assert.AreEqual(quarterAngles[i].Turns, quarterAngles[j].Turns, DELTA);
                    Assert.AreEqual(quarterAngles[i].Degrees, quarterAngles[j].Degrees, DELTA);
                    Assert.AreEqual(quarterAngles[i].Radians, quarterAngles[j].Radians, DELTA);
                }
            }
        }

        [TestMethod]
        public void EqualsTest()
        {
            var a = new Angle(135, AngleUnit.Degrees);
            var b = new Angle(0.4, AngleUnit.Turns);
            var c = new Angle(144, AngleUnit.Degrees);
            var d = new Angle(0.40000000001, AngleUnit.Turns);

            Assert.IsTrue(a.Equals(a));
            Assert.IsFalse(a.Equals(b));
            Assert.IsTrue(b.Equals(c));
            Assert.IsTrue(d.Equals(c, DELTA));

            Assert.AreEqual(0, b.CompareTo(c));
            Assert.IsTrue(a.CompareTo(c) < 0);
            Assert.IsTrue(c.CompareTo(a) > 0);
            Assert.AreEqual(0, b.CompareTo(d, DELTA));

            Assert.IsFalse(a == b);
            Assert.IsTrue(b == c);
            Assert.IsTrue(a != b);
            Assert.IsFalse(b != c);

            Assert.IsTrue(a < c);
            Assert.IsTrue(a <= c);
            Assert.IsFalse(b < c);
            Assert.IsTrue(b <= c);

            Assert.AreEqual(0.55, (b + new Angle(0.15, AngleUnit.Turns)).Turns, DELTA);
            Assert.AreEqual(0.25, (b - new Angle(0.15, AngleUnit.Turns)).Turns, DELTA);
            Assert.AreEqual(0.5, (b * 1.25).Turns, DELTA);
            Assert.AreEqual(0.1, (b / 4).Turns, DELTA);
        }

        [TestMethod]
        public void NonCanonicalTest()
        {
            var a = new Angle(0.25, AngleUnit.Turns);
            var b = new Angle(1.25, AngleUnit.Turns);

            Assert.AreNotEqual(a, b);
            Assert.AreEqual(a, b.Canonical);
        }

        [TestMethod]
        public void TrigTest()
        {
            double theta = Math.PI * 1.5;
            var angle = new Angle(theta, AngleUnit.Radians);

            Assert.AreEqual(Math.Cos(theta), angle.Cos(), DELTA);
            Assert.AreEqual(Math.Sin(theta), angle.Sin(), DELTA);
            Assert.AreEqual(Math.Tan(theta), angle.Tan(), DELTA);
            Assert.AreEqual(Math.Cosh(theta), angle.Cosh(), DELTA);
            Assert.AreEqual(Math.Sinh(theta), angle.Sinh(), DELTA);
            Assert.AreEqual(Math.Tanh(theta), angle.Tanh(), DELTA);

            Assert.AreEqual(new Angle(Math.Acos(0.5), AngleUnit.Radians).Turns, Angle.ArcCos(0.5).Turns, DELTA);
            Assert.AreEqual(new Angle(Math.Asin(0.5), AngleUnit.Radians).Turns, Angle.ArcSin(0.5).Turns, DELTA);
            Assert.AreEqual(new Angle(Math.Atan(0.5), AngleUnit.Radians).Turns, Angle.ArcTan(0.5).Turns, DELTA);
            Assert.AreEqual(new Angle(Math.Atan2(2, 1), AngleUnit.Radians).Turns, Angle.ArcTan2(2, 1).Turns, DELTA);
        }

        [TestMethod]
        public void ToPointTest()
        {
            var up = new Angle(0.25, AngleUnit.Turns);
            var left = new Angle(0.5, AngleUnit.Turns);
            double x, y;

            (x, y) = up.ToPoint();
            Assert.AreEqual(0, x, DELTA);
            Assert.AreEqual(1, y, DELTA);

            (x, y) = left.ToPoint();
            Assert.AreEqual(-1, x, DELTA);
            Assert.AreEqual(0, y, DELTA);


            (x, y) = left.ToPoint(2.5);
            Assert.AreEqual(-2.5, x, DELTA);
            Assert.AreEqual(0, y, DELTA);

            (x, y) = up.ToPoint(4, 4, 2);
            Assert.AreEqual(4, x, DELTA);
            Assert.AreEqual(6, y, DELTA);
        }

        [TestMethod]
        public void CastTest()
        {
            var p = new Portion(0.4);
            var angle = new Angle(0.4, AngleUnit.Turns);

            Assert.AreEqual(angle, (Angle)p);
            Assert.AreEqual(p, (Portion)angle);
        }
    }
}
