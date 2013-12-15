using System;
using System.Collections.Generic;

namespace ObjectCompare.Expressions
{
    public static class CompiledObjectComparer<T>
    {
        private static readonly Func<T, T, ObjectPairHashSet, bool> Query;

        static CompiledObjectComparer()
        {
            Query = ExpressionBuilder.CompileQuery<T>(ObjectComparer.Settings);
        }

        public static bool Equals(T x, T y)
        {
            var visitedObjects = new ObjectPairHashSet();

            return Query(x, y, visitedObjects);
        }

        public static bool Equals(T x, T y, ObjectPairHashSet visitedObjects)
        {
            return Query(x, y, visitedObjects);
        }

        public static bool CollectionEquals(ICollection<T> x, ICollection<T> y, ObjectPairHashSet visitedObjects)
        {
            if (x.Count != y.Count)
                return false;

            IEnumerable<T> e1 = x;
            IEnumerable<T> e2 = y;

            return EnumerableEquals(e1, e2, visitedObjects);
        }

        public static bool EnumerableEquals(IEnumerable<T> x, IEnumerable<T> y, ObjectPairHashSet visitedObjects)
        {
            var enumerator1 = x.GetEnumerator();
            var enumerator2 = y.GetEnumerator();

            while (true)
            {
                var m1 = enumerator1.MoveNext();
                var m2 = enumerator2.MoveNext();

                if (m1 != m2)
                    return false;

                if (m1 == false)
                    return true;

                if (!Equals(enumerator1.Current, enumerator2.Current, visitedObjects))
                    return false;
            }
        }
    }
}