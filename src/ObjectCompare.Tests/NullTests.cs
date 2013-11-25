using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObjectCompare.Tests
{
    [TestClass]
    public class NullTests
    {
        [TestMethod]
        public void Null_values()
        {
            var r = ObjectComparer.Equals<string>(null, null);

            Assert.IsTrue(r);
        }

        [TestMethod]
        public void Mismatched_null_values()
        {
            var r1 = ObjectComparer.Equals(null, "y");
            var r2 = ObjectComparer.Equals("x", null);

            Assert.IsFalse(r1);
            Assert.IsFalse(r2);
        }
    }
}