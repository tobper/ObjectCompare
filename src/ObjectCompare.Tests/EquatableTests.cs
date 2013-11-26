using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObjectCompare.Tests
{
    [TestClass]
    public class EquatableTests
    {
        [TestInitialize]
        public void Initialize()
        {
            ObjectComparer.Settings = new ObjectComparerSettings();
        }

        [TestMethod]
        public void Matched_equatables()
        {
            var x = new Foo { Number = 5, IsEqual = true };
            var y = new Foo { Number = 7, IsEqual = true };
            var r = ObjectComparer.Equals(x, y);

            Assert.IsTrue(r);
        }

        [TestMethod]
        public void Mismatched_equatables()
        {
            var x = new Foo { Number = 5, IsEqual = false };
            var y = new Foo { Number = 5, IsEqual = false };
            var r = ObjectComparer.Equals(x, y);

            Assert.IsFalse(r);
        }

        public class Foo : IEquatable<Foo>
        {
            public bool Equals(Foo other)
            {
                return IsEqual;
            }

            public int Number { get; set; }
            public bool IsEqual { get; set; }
        }
    }
}