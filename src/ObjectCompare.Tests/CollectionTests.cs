using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObjectCompare.Tests
{
    [TestClass]
    public class CollectionTests
    {
        [TestMethod]
        public void Collections_of_different_lengths1()
        {
            var x = new Foo { Items = new[] { 0 } };
            var y = new Foo { Items = new[] { 0, 1 } };
            var r = ObjectComparer.Equals(x, y);

            Assert.IsFalse(r);
        }

        [TestMethod]
        public void Collections_of_different_lengths2()
        {
            var x = new Foo { Items = new[] { 0, 1 } };
            var y = new Foo { Items = new[] { 0 } };
            var r = ObjectComparer.Equals(x, y);

            Assert.IsFalse(r);
        }

        [TestMethod]
        public void Collections_with_different_items()
        {
            var x = new Foo { Items = new[] { 0, 2 } };
            var y = new Foo { Items = new[] { 0, 1 } };
            var r = ObjectComparer.Equals(x, y);

            Assert.IsFalse(r);
        }

        [TestMethod]
        public void Collections_with_same_items()
        {
            var x = new Foo { Items = new[] { 0, 1 } };
            var y = new Foo { Items = new[] { 0, 1 } };
            var r = ObjectComparer.Equals(x, y);

            Assert.IsTrue(r);
        }

        class Foo
        {
            public ICollection<int> Items { get; set; }
        }
    }
}