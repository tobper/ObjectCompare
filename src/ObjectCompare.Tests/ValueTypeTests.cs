using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObjectCompare.Tests
{
    [TestClass]
    public class ValueTypeTests
    {
        [TestMethod]
        public void Matches_value_types()
        {
            var i = ObjectComparer.Equals(5, 5);
            var l = ObjectComparer.Equals(5L, 5L);
            var d = ObjectComparer.Equals(5d, 5d);
            var m = ObjectComparer.Equals(5m, 5m);

            Assert.IsTrue(i);
            Assert.IsTrue(l);
            Assert.IsTrue(d);
            Assert.IsTrue(m);
        }

        [TestMethod]
        public void Mismatched_value_types()
        {
            var i = ObjectComparer.Equals(5, 7);
            var l = ObjectComparer.Equals(5L, 7L);
            var d = ObjectComparer.Equals(5d, 7d);
            var m = ObjectComparer.Equals(5m, 7m);

            Assert.IsFalse(i);
            Assert.IsFalse(l);
            Assert.IsFalse(d);
            Assert.IsFalse(m);
        }
    }
}
