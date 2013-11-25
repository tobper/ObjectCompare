namespace ObjectCompare.Tests.Types
{
    class MemberFoo
    {
        private readonly int _field;

        public MemberFoo(int publicProperty, int protectedProperty, int field)
        {
            _field = field;
            PublicProperty = publicProperty;
            ProtectedProperty = protectedProperty;
        }

        public int PublicProperty { get; private set; }
        protected int ProtectedProperty { get; private set; }
    }
}