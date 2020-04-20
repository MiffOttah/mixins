using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiffTheFox.Text;
using System.Linq;

namespace MixinTests
{
    [TestClass]
    public class QueryStringTests
    {
        [TestMethod]
        public void ParseTest()
        {
            var q = new QueryString("foo=bar&baz=Lorem%20Ipsum");

            Assert.AreEqual(2, q.Count);
            Assert.IsTrue(q.ContainsKey("foo"));
            Assert.IsFalse(q.ContainsKey("abcdefg"));
            CollectionAssert.AreEqual(new string[] { "foo", "baz" }, q.Keys.ToArray());

            Assert.AreEqual("bar", q["foo"]);
            Assert.AreEqual("Lorem Ipsum", q["baz"]);
            Assert.IsNull(q["qux"]);
        }

        [TestMethod]
        public void GetValueTest()
        {
            var q = new QueryString("someString=Hello%2C+world!&someInt=42&someBool=true");

            Assert.AreEqual("Hello, world!", q.GetString("someString"));
            Assert.AreEqual("ABC", q.GetString("not-there", "ABC"));
            Assert.IsNull(q.GetString("NonExistant"));

            Assert.AreEqual(42, q.GetInt("someInt"));
            Assert.AreEqual(-14, q.GetInt("NonExistant", -14));
            Assert.AreEqual(0, q.GetInt("not-there"));
            Assert.AreEqual(100, q.GetInt("someString", 100));

            Assert.IsTrue(q.GetBoolean("someBool"));
            Assert.IsTrue(q.GetBoolean("not-there", true));
            Assert.IsFalse(q.GetBoolean("not-there"));
            Assert.IsTrue(q.GetBoolean("someString"));
            Assert.IsTrue(q.GetBoolean("someInt"));
        }

        [TestMethod]
        public void GetBooleanTest()
        {
            var q = new QueryString("a&b=&c=0&d=false&e=no&f=+FALSE+&g=%0d%0a");
            foreach (string k in q.Keys)
            {
                Assert.IsFalse(q.GetBoolean(k, true));
            }
        }

        [TestMethod]
        public void OverlayTest()
        {
            var x = new QueryString("abc=1");
            var y = new QueryString("def=2");
            x.Overlay(y);

            Assert.AreEqual(2, x.Count);
            Assert.AreEqual(1, y.Count);

            Assert.IsTrue(x.ContainsKey("abc"));
            Assert.IsTrue(x.ContainsKey("def"));
            Assert.IsFalse(y.ContainsKey("abc"));
            Assert.IsTrue(y.ContainsKey("def"));
        }

        [TestMethod]
        public void ConcatOperatorTest()
        {
            var x = new QueryString("abc=1");
            var y = new QueryString("def=2&ghi=3");
            var z = x + y;

            Assert.AreEqual(1, x.Count);
            Assert.AreEqual(2, y.Count);
            Assert.AreEqual(3, z.Count);

            CollectionAssert.AreEquivalent(z.Keys.ToArray(), x.Keys.Concat(y.Keys).ToArray());
            CollectionAssert.AreEquivalent((x + null).Keys.ToArray(), x.Keys.ToArray());
            CollectionAssert.AreEquivalent((null + y).Keys.ToArray(), y.Keys.ToArray());

            QueryString null1 = null, null2 = null;
            var z2 = null1 + null2;

            Assert.IsNotNull(z2);
            Assert.AreEqual(0, z2.Count);
        }
    }
}
