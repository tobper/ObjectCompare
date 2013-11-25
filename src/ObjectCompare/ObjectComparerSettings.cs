namespace ObjectCompare
{
    public class ObjectComparerSettings
    {
        public ObjectComparerSettings()
        {
            UseEquatable = true;
            UseHashCodes = true;
            PublicProperties = true;
        }

        public bool PrivateFields { get; set; }
        public bool PublicFields { get; set; }
        public bool ProtectedProperties { get; set; }
        public bool PublicProperties { get; set; }
        public bool UseEquatable { get; set; }
        public bool UseHashCodes { get; set; }
    }
}