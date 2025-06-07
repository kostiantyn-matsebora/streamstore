namespace StreamStore.Sql.Tests.PostgreSql.Storage
{
    [CollectionDefinition("Reading Postgres")]
    public class Reading_collection_postgres : ICollectionFixture<PostgresStorageFixture>
    {
        public Reading_collection_postgres() : base()
        {
        }
    }

    [CollectionDefinition("Deleting Postgres")]
    public class Deleting_collection_postgres : ICollectionFixture<PostgresStorageFixture>
    {
        public Deleting_collection_postgres() : base()
        {
        }
    }

    [CollectionDefinition("Writing Postgres")]
    public class Writing_collection_postgres : ICollectionFixture<PostgresStorageFixture>
    {
        public Writing_collection_postgres() : base()
        {
        }
    }
}
