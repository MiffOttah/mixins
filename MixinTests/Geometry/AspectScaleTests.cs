using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiffTheFox.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixinTests.Geometry
{
    [TestClass]
    public class AspectScaleTests
    {
        [TestMethod]
        [DataRow(100, 10, 5, 100, 50)]
        [DataRow(100, 5, 10, 50, 100)]
        [DataRow(10, 100, 40, 10, 4)]
        [DataRow(10, 40, 100, 4, 10)]
        [DataRow(100, 50, 50, 100, 100)]
        [DataRow(100, 100, 50, 100, 50)]
        [DataRow(100, 50, 100, 50, 100)]
        [DataRow(100, 100, 100, 100, 100)]
        [DataRow(100, 200, 100, 100, 50)]
        [DataRow(100, 100, 200, 50, 100)]
        public void TestScaleToFixBox(int targetSize, int srcWidth, int srcHeight, int expectedScaledWidth, int expectedScaledHeight)
        {
            AspectScale.ScaleToFitSquare(targetSize, srcWidth, srcHeight, out int scaledWidth, out int scaledHeight);
            Assert.AreEqual(expectedScaledWidth, scaledWidth);
            Assert.AreEqual(expectedScaledHeight, scaledHeight);
        }
    }
}
