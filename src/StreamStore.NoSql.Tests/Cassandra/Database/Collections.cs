namespace StreamStore.NoSql.Tests.Cassandra.Database
{
    [CollectionDefinition("Reading Cassandra")]
    public class Reading_collection : ICollectionFixture<CassandraDatabaseFixture>
    {
        public Reading_collection() : base()
        {
        }
    }

    [CollectionDefinition("Deleting Cassandra")]
    public class Deleting_collection : ICollectionFixture<CassandraDatabaseFixture>
    {
        public Deleting_collection() : base()
        {
        }
    }

    [CollectionDefinition("Writing Cassandra")]
    public class Writing_collection : ICollectionFixture<CassandraDatabaseFixture>
    {
        public Writing_collection() : base()
        {
        }
    }
}
