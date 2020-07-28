using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiffTheFox.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixinTests.Text
{
    [TestClass]
    public class GlobTests
    {
        [TestMethod]
        [DataRow(true, "", "")]
        [DataRow(false, "x", "")]
        [DataRow(false, "", "x")]
        [DataRow(true, "abc", "abc")]
        [DataRow(true, "*", "abc")]
        [DataRow(true, "*c", "abc")]
        [DataRow(false, "*b", "abc")]
        [DataRow(true, "a*", "abc")]
        [DataRow(false, "b*", "abc")]
        [DataRow(true, "a*", "a")]
        [DataRow(true, "*a", "a")]
        [DataRow(true, "a*b*c*d*e*", "axbxcxdxe")]
        [DataRow(true, "a*b*c*d*e*", "axbxcxdxexxx")]
        [DataRow(true, "a*b?c*x", "abxbbxdbxebxczzx")]
        [DataRow(false, "a*b?c*x", "abxbbxdbxebxczzy")]
        [DataRow(false, "a*a*a*a*b", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
        [DataRow(true, "*x", "xxx")]
        public void TestGlobMatch(bool expected, string glob, string text)
        {
            
        }
    }
}
