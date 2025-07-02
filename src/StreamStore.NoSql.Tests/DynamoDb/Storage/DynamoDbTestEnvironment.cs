using StreamStore.Testing.StreamStorage;

namespace StreamStore.NoSql.Tests.DynamoDb.Storage
{
    public class DynamoDbTestEnvironment : StorageFixtureTestEnvironmentBase
    {
        public DynamoDbTestEnvironment() : this(new DynamoDbStorageFixture())
        {
        }

        public DynamoDbTestEnvironment(DynamoDbStorageFixture fixture) : base(fixture)
        {
        }


        protected override void Initialize()
        {
        }
    }
}
