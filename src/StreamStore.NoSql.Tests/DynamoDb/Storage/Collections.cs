namespace StreamStore.NoSql.Tests.DynamoDb.Storage
{
    [CollectionDefinition("Reading DynamoDb")]
    public class Reading_collection : ICollectionFixture<DynamoDbStorageFixture>
    {
        public Reading_collection() : base()
        {
        }
    }

    [CollectionDefinition("Deleting DynamoDb")]
    public class Deleting_collection : ICollectionFixture<DynamoDbStorageFixture>
    {
        public Deleting_collection() : base()
        {
        }
    }

    [CollectionDefinition("Writing DynamoDb")]
    public class Writing_collection : ICollectionFixture<DynamoDbStorageFixture>
    {
        public Writing_collection() : base()
        {
        }
    }
}
