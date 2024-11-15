namespace StreamStore.Sql.Tests.PostgreSql.Database
{
    [CollectionDefinition("Reading Postgres")]
    public class Reading_collection_postgres : ICollectionFixture<PostgresDatabaseFixture>
    {
        public Reading_collection_postgres() : base()
        {
        }
    }

    [CollectionDefinition("Deleting Postgres")]
    public class Deleting_collection_postgres : ICollectionFixture<PostgresDatabaseFixture>
    {
        public Deleting_collection_postgres() : base()
        {
        }
    }

    [CollectionDefinition("Writing Postgres")]
    public class Writing_collection_postgres : ICollectionFixture<PostgresDatabaseFixture>
    {
        public Writing_collection_postgres() : base()
        {
        }
    }
}
