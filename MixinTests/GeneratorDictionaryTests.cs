using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiffTheFox;

namespace MixinTests
{
    [TestClass]
    public class GeneratorDictionaryTests
    {
        [TestMethod]
        public void SimpleDictionaryTest()
        {
            var d = new GeneratorDictionary<int, string>
            {
                { 1, "Hello" },
                { 2, "World" }
            };
            Assert.AreEqual(2, d.Count);
            Assert.AreEqual("World", d[2]);
        }

        [TestMethod]
        public void GenerationTest()
        {
            var d = new GeneratorDictionary<string, string>();
            d.ItemGeneration += (sender, e) => e.Result = e.Key.ToUpperInvariant();
            d.Add("Test", "Some String");

            Assert.AreEqual(1, d.Count);
            Assert.AreEqual("Some String", d["Test"]);

            Assert.AreEqual("ABC", d["abc"]);
            Assert.AreEqual("DEF", d["Def"]);

            Assert.AreEqual(3, d.Count);
            CollectionAssert.AreEquivalent(new string[] { "Test", "abc", "Def" }, (System.Collections.ICollection)d.Keys);
        }

        [TestMethod]
        public void GenerationSpecifiedInConstructorTest()
        {
            var d = new GeneratorDictionary<string, string>(k => k.ToLowerInvariant())
            {
                { "test", "unrelated" }
            };

            Assert.AreEqual("unrelated", d["test"]);
            Assert.AreEqual("testtwo", d["TestTwo"]);
        }

        [TestMethod]
        public void FailedGenerationTest()
        {
            var d = new GeneratorDictionary<string, int>();
            d.ItemGeneration += (sender, e) =>
            {
                bool parsed = int.TryParse(e.Key, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int n);
                e.Result = n;
                e.ResultAvailable = parsed;
            };

            Assert.AreEqual(42, d["42"]);
            Assert.AreEqual(123, d["123"]);

            Assert.IsTrue(d.TryGetValue("104", out int test));
            Assert.AreEqual(104, test);

            Assert.IsFalse(d.TryGetValue("asdf", out test));
            Assert.IsFalse(d.ContainsKey("asdf"));

            Assert.ThrowsException<KeyNotFoundException>(() => test = d["not an integer"]);
        }

        [TestMethod]
        public void GenreatorThrowsExceptionTest()
        {
            var d = new GeneratorDictionary<int, int>(k => throw new Exception());
            Assert.ThrowsException<Exception>(() => d[42]);
            d[123] = 456;
            Assert.AreEqual(456, d[123]);
        }
    }
}
