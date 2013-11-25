namespace ObjectCompare
{
    public static class ObjectComparerExtensions
    {
        /// <summary>
        /// Calls <see cref="ObjectCompare.ObjectComparer.Equals"/> to perform a recursive compare of the objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if x equals y; otherwise false.</returns>
        public static bool ObjectEquals<T>(this T x, T y)
        {
            return ObjectComparer.Equals(x, y);
        }
    }
}