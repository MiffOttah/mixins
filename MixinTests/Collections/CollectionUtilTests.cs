using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiffTheFox.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixinTests.Collections
{
    [TestClass]
    public class CollectionUtilTests
    {
        [TestMethod]
        [DataRow("1,2,3,4", "1,2;3,4")]
        [DataRow("A,B,C", "A;B;C")]
        [DataRow("A,B,C,D", "A,B,C,D", DisplayName = "With single element")]
        [DataRow("1,2,3", "1;;2,3", DisplayName = "With empty element")]
        [DataRow("1,2,3", "1;null;2,3", DisplayName = "With null element")]
        [DataRow("", null, DisplayName = "With no elements")]
        [DataRow("", ";;null", DisplayName = "With only empty/null elements")]
        public void ConcatTest(string expected, string sources)
        {
            var l = new List<string[]>();
            if (sources != null)
            {
                foreach (string source in sources.Split(';'))
                {
                    switch (source)
                    {
                        case "null":
                            l.Add(null);
                            break;

                        case "":
                            l.Add(new string[0]);
                            break;

                        default:
                            l.Add(source.Split(','));
                            break;
                    }
                }
            }

            var expectedArr = expected == "" ? new string[0] : expected.Split(',');
            var actual = CollectionUtil.Concat(l.ToArray());
            CollectionAssert.AreEqual(expectedArr, actual);
        }
    }
}
