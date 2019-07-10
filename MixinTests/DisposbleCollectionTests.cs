using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiffTheFox;
using MiffTheFox.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixinTests
{
    [TestClass]
    public class DisposbleCollectionTests
    {
        class MockDisposable : IDisposable
        {
            public bool Disposed { get; private set; } = false;

            public void Dispose()
            {
                Disposed = true;
            }
        }

        [TestMethod]
        public void TestMock()
        {
            var d1 = new MockDisposable();
            Assert.IsFalse(d1.Disposed);
            d1.Dispose();
            Assert.IsTrue(d1.Disposed);
        }

        [TestMethod]
        public void TestBasicDisposal()
        {
            var d1 = new MockDisposable();
            var d2 = new MockDisposable();

            Assert.IsFalse(d1.Disposed);
            Assert.IsFalse(d2.Disposed);

            var coll = new DisposableCollection { d1, d2 };
            coll.Dispose();

            Assert.IsTrue(d1.Disposed);
            Assert.IsTrue(d2.Disposed);
        }

        [TestMethod]
        public void TestObjectDisposedException()
        {
            var c = new DisposableCollection();

            c.Dispose();
            c.Dispose(); // second call should have no effect

            Assert.ThrowsException<ObjectDisposedException>(() =>
                c.Add(new MockDisposable()));

            Assert.ThrowsException<ObjectDisposedException>(() =>
                c.Clear());

            Assert.ThrowsException<ObjectDisposedException>(() =>
                c.Contains(new MockDisposable()));

            Assert.ThrowsException<ObjectDisposedException>(() =>
                c.GetEnumerator());

            Assert.ThrowsException<ObjectDisposedException>(() =>
                c.Remove(new MockDisposable()));
        }

        [TestMethod]
        public void TestInvalidOperations()
        {
            var c = new DisposableCollection();

            var d1 = new MockDisposable();

            // adding a reference to the same object twice
            c.Add(d1);
            Assert.ThrowsException<ArgumentException>(() =>
                c.Add(d1));

            // adding a null reference
            Assert.ThrowsException<ArgumentNullException>(() =>
                c.Add(null));
        }

        [TestMethod]
        public void TestDisposeOnRemove()
        {
            (DisposableCollection, MockDisposable) prepareSet()
            {
                var d = new MockDisposable();
                var c = new DisposableCollection { d };
                return (c, d);
            }

            var (c1, d1) = prepareSet();
            c1.Remove(d1);
            Assert.IsTrue(d1.Disposed);

            (c1, d1) = prepareSet();
            c1.Remove(d1, true);
            Assert.IsTrue(d1.Disposed);

            (c1, d1) = prepareSet();
            c1.Remove(d1, false);
            Assert.IsFalse(d1.Disposed);
        }

        [TestMethod]
        public void TestDisposeOnClear()
        {
            (DisposableCollection, MockDisposable) prepareSet()
            {
                var d = new MockDisposable();
                var c = new DisposableCollection { d };
                return (c, d);
            }

            var (c1, d1) = prepareSet();
            c1.Clear();
            Assert.IsTrue(d1.Disposed);

            (c1, d1) = prepareSet();
            c1.Clear(true);
            Assert.IsTrue(d1.Disposed);

            (c1, d1) = prepareSet();
            c1.Clear(false);
            Assert.IsFalse(d1.Disposed);
        }
    }
}
