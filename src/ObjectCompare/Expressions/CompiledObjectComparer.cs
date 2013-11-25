using System;

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
    }
}