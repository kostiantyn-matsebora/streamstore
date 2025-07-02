
namespace StreamStore.NoSql.Tests.Cassandra.CosmosDb.Storage
{
    [CollectionDefinition("Reading Cassandra CosmosDb")]
    public class Reading_collection : ICollectionFixture<CosmosDbStorageFixture>
    {
        public Reading_collection() : base()
        {
        }
    }

    [CollectionDefinition("Deleting Cassandra CosmosDb")]
    public class Deleting_collection : ICollectionFixture<CosmosDbStorageFixture>
    {
        public Deleting_collection() : base()
        {
        }
    }

    [CollectionDefinition("Writing Cassandra CosmosDb")]
    public class Writing_collection : ICollectionFixture<CosmosDbStorageFixture>
    {
        public Writing_collection() : base()
        {
        }
    }
}
