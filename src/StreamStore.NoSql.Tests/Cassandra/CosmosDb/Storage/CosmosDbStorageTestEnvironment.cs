using StreamStore.Testing.StreamStorage;

namespace StreamStore.NoSql.Tests.Cassandra.CosmosDb.Storage
{
    public class CosmosDbStorageTestEnvironment : StorageFixtureTestEnvironmentBase
    {

        public CosmosDbStorageTestEnvironment() : this(new CosmosDbStorageFixture())
        {
        }

        public CosmosDbStorageTestEnvironment(CosmosDbStorageFixture fixture) : base(fixture)
        {
        }


        protected override void Initialize()
        {
        }
    }
}
