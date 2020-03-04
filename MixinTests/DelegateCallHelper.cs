using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MixinTests
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
}
