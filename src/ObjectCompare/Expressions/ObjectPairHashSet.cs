using System.Collections.Generic;

namespace ObjectCompare.Expressions
{
    public class ObjectPairHashSet : HashSet<ObjectPair>
    {
        public ObjectPairHashSet() :
            base(new ObjectPairComparer())
        {
        }

        class ObjectPairComparer : IEqualityComparer<ObjectPair>
        {
            public bool Equals(ObjectPair x, ObjectPair y)
            {
                return
                    ReferenceEquals(x.Object1, y.Object1) &&
                    ReferenceEquals(x.Object2, y.Object2);
            }

            public int GetHashCode(ObjectPair pair)
            {
                return pair.GetHashCode();
            }
        }
    }
}