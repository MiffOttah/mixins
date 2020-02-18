using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiffTheFox;

namespace MixinTests
{
    [TestClass]
    public class TruthinessExtensionsTests
    {
        // for testing if a method is called
        class DelegateCallHelper
        {
            public bool Called { get; private set; } = false;

            public void Call()
            {
                Called = true;
            }

            public T Call<T>(T value)
            {
                Called = true;
                return value;
            }

            public void AssertCalled() => Assert.IsTrue(Called);
            public void AssertNotCalled() => Assert.IsFalse(Called);
            public void AssertCalled(bool expected) => Assert.AreEqual(expected, Called);
            public void Reset() => Called = false;

            //public static void AssertCall(Action<DelegateCallHelper> test)
            //{
            //    var h = new DelegateCallHelper();
            //    test(h);
            //    h.AssertCalled();
            //}

            //public static void AssertNotCall(Action<DelegateCallHelper> test)
            //{
            //    var h = new DelegateCallHelper();
            //    test(h);
            //    h.AssertNotCalled();
            //}
        }

        [TestMethod]
        [DataRow(true, 1)]
        [DataRow(true, "value")]
        [DataRow(false, "")]
        [DataRow(false, null)]
        public void IfTest(bool expected, object obj)
        {
            var callThen = new DelegateCallHelper();
            var callElse = new DelegateCallHelper();

            obj.If(() => callThen.Call(), () => callElse.Call());

            callThen.AssertCalled(expected);
            callElse.AssertCalled(!expected);
        }

        [TestMethod]
        [DataRow(true, 1)]
        [DataRow(true, "value")]
        [DataRow(false, "")]
        [DataRow(false, null)]
        public void IfTTest(bool expected, object obj)
        {
            var callThen = new DelegateCallHelper();
            var callElse = new DelegateCallHelper();

            obj.If(x => {
                Assert.AreEqual(obj, x);
                callThen.Call();
            }, x => {
                Assert.AreEqual(obj, x);
                callElse.Call();
            });

            callThen.AssertCalled(expected);
            callElse.AssertCalled(!expected);
        }

        [TestMethod]
        [DataRow(true, 1)]
        [DataRow(true, "value")]
        [DataRow(false, "")]
        [DataRow(false, null)]
        public void IfTTTest(bool expected, object obj)
        {
            var callThen = new DelegateCallHelper();
            var callElse = new DelegateCallHelper();

            int r = obj.If(x => {
                Assert.AreEqual(obj, x);
                callThen.Call();
                return -1;
            }, x => {
                Assert.AreEqual(obj, x);
                callElse.Call();
                return 1;
            });

            callThen.AssertCalled(expected);
            callElse.AssertCalled(!expected);
            Assert.AreEqual(expected ? -1 : 1, r);
        }

        [TestMethod]
        public void IfElseNullTest()
        {
            var callThen = new DelegateCallHelper();
            0.If(callThen.Call);
            callThen.AssertNotCalled();
        }

        [TestMethod]
        public void IfThenNullTest()
        {
            var callElse = new DelegateCallHelper();
            1.If(null, callElse.Call);
            callElse.AssertNotCalled();
        }

        [TestMethod]
        public void IfThenDefaultResultTest()
        {
            int r = 2.If((Func<int, int>)null);
            Assert.AreEqual(0, r);
        }

        [TestMethod]
        public void IfElseDefaultResultTest()
        {
            string v = "".If(x => "WRONG");
            Assert.IsNull(v);
        }

        [TestMethod]
        [DataRow(1, 0, 1)]
        [DataRow(10, 10, 1)]
        [DataRow("test", "test", "else")]
        [DataRow("else", "", "else")]
        [DataRow("else", null, "else")]
        [DataRow(0, 0, 0)]
        public void TestOr(object expected, object x, object y)
        {
            Assert.AreEqual(expected, x.Or(y));
        }
    }
}
