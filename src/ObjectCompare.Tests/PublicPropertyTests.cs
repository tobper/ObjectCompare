using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObjectCompare.Tests
{
    [TestClass]
    public class PublicPropertyTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ObjectComparer.Settings = new ObjectComparerSettings();
        }

        [TestMethod]
        public void Public_matched_properties()
        {
            var x = new Foo(5, 5, 5);
            var y = new Foo(5, 7, 7);
            var r = ObjectComparer.Equals(x, y);

            Assert.IsTrue(r);
        }

        [TestMethod]
        public void Public_mismatched_properties()
        {
            var x = new Foo(5, 5, 5);
            var y = new Foo(7, 7, 7);
            var r = ObjectComparer.Equals(x, y);

            Assert.IsTrue(r);
        }

        class Foo : Types.MemberFoo
        {
            public Foo(int publicProperty, int protectedProperty, int field) :
                base(publicProperty, protectedProperty, field)
            {
            }
        }
    }
}