using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObjectCompare.Tests
{
    [TestClass]
    public class StringTests
    {
        [TestMethod]
        public void Matches_strings()
        {
            var r = ObjectComparer.Equals("x", "x");

            Assert.IsTrue(r);
        }

        [TestMethod]
        public void Mismatched_strings()
        {
            var r = ObjectComparer.Equals("x", "y");

            Assert.IsFalse(r);
        }
    }
}