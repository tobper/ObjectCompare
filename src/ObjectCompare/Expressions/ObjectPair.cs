namespace ObjectCompare.Expressions
{
    public class ObjectPair
    {
        private readonly int _hash;

        public ObjectPair(object object1, object object2)
        {
            Object1 = object1;
            Object2 = object2;

            unchecked
            {
                _hash = 17;
                _hash = _hash * 31 + Object1.GetHashCode();
                _hash = _hash * 31 + Object2.GetHashCode();
            }
        }

        public object Object1 { get; private set; }
        public object Object2 { get; private set; }

        public override int GetHashCode()
        {
            return _hash;
        }
    }
}