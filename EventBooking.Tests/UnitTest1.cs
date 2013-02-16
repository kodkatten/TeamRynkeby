using System;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EventBooking.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestOurMagicalRegex()
        {
            const string pattern = @"\d{3}\s{0,1}\d{2}";

            Assert.IsTrue(Regex.Match("123 45", pattern).Success);
            Assert.IsTrue(Regex.Match("12345", pattern).Success);
            Assert.IsFalse(Regex.Match("1 2345", pattern).Success);
            Assert.IsFalse(Regex.Match("12 345", pattern).Success);
            Assert.IsFalse(Regex.Match("abc de", pattern).Success);
            Assert.IsFalse(Regex.Match("abcde", pattern).Success);
        }
    }
}
