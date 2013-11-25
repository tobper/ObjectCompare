using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectCompare.Tests.Types;

namespace ObjectCompare.Tests
{
    [TestClass]
    public class PrivateFieldTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ObjectComparer.Settings = new ObjectComparerSettings
            {
                PublicProperties = false,
                PrivateFields = true
            };
        }

        [TestMethod]
        public void Private_matched_fields()
        {
            var x = new Foo(5, 5, 5);
            var y = new Foo(7, 7, 5);
            var r = ObjectComparer.Equals(x, y);

            Assert.IsTrue(r);
        }

        [TestMethod]
        public void Private_mismatched_fields()
        {
            var x = new Foo(5, 5, 5);
            var y = new Foo(7, 7, 7);
            var r = ObjectComparer.Equals(x, y);

            Assert.IsTrue(r);
        }

        class Foo : MemberFoo
        {
            public Foo(int publicProperty, int protectedProperty, int field) :
                base(publicProperty, protectedProperty, field)
            {
            }
        }
    }
}