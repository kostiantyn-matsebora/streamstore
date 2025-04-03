namespace StreamStore.NoSql.Tests.Cassandra.Storage
{
    [CollectionDefinition("Reading Cassandra")]
    public class Reading_collection : ICollectionFixture<CassandraStorageFixture>
    {
        public Reading_collection() : base()
        {
        }
    }

    [CollectionDefinition("Deleting Cassandra")]
    public class Deleting_collection : ICollectionFixture<CassandraStorageFixture>
    {
        public Deleting_collection() : base()
        {
        }
    }

    [CollectionDefinition("Writing Cassandra")]
    public class Writing_collection : ICollectionFixture<CassandraStorageFixture>
    {
        public Writing_collection() : base()
        {
        }
    }
}
