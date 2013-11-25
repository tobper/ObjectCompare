using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObjectCompare.Tests
{
    [TestClass]
    public class BackReferenceTests
    {
        [TestMethod]
        public void Back_refence_with_different_levels()
        {
            var x = new Foo();
            var y = new Foo();

            x.Bar = new Bar { Foo = x };
            y.Bar = new Bar { Foo = new Foo { Bar = new Bar { Foo = y } } };

            var r = ObjectComparer.Equals(x, y);

            Assert.IsTrue(r);
        }

        [TestMethod]
        public void Back_refence_with_one_level()
        {
            var x = new Foo();
            var y = new Foo();

            x.Bar = new Bar { Foo = x };
            y.Bar = new Bar { Foo = y };

            var r = ObjectComparer.Equals(x, y);

            Assert.IsTrue(r);
        }

        private class Foo
        {
            public Bar Bar { get; set; }
        }

        private class Bar
        {
            public Foo Foo { get; set; }
        }
    }
}
