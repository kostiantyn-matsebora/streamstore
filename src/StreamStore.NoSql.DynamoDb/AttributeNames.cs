
namespace StreamStore.NoSql.DynamoDb
{
    internal static class AttributeNames
    {
        public const string StreamId = "StreamId";
        public const string Id = "Id";
        public const string Revision = "Revision";
        public const string Timestamp = "Timestamp";
        public const string Data = "Data";
        public const string CustomPropertyPrefix = "_Property_";
        public static string CustomProperty(string propertyName) => $"{CustomPropertyPrefix}{propertyName}";
    }
}
