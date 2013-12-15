using System;
using System.Collections;
using System.Collections.Generic;
using ObjectCompare.Expressions;

namespace ObjectCompare
{
    public class ObjectComparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y)
        {
            return CompiledObjectComparer<T>.Equals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }

    public static class ObjectComparer
    {
        private static ObjectComparerSettings _settings;

        static ObjectComparer()
        {
            Settings = new ObjectComparerSettings();
        }

        public static ObjectComparerSettings Settings
        {
            get { return _settings; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                _settings = value;
            }
        }

        public static bool Equals<T>(T x, T y)
        {
            return CompiledObjectComparer<T>.Equals(x, y);
        }
    }
}