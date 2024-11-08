namespace StreamStore.Sql.Tests.Sqlite.Database
{
    [CollectionDefinition("Reading")]
    public class Reading_collection : ICollectionFixture<SqliteDatabaseFixture>
    {
        public Reading_collection() : base()
        {
        }
    }

    [CollectionDefinition("Deleting")]
    public class Deleting_collection : ICollectionFixture<SqliteDatabaseFixture>
    {
        public Deleting_collection() : base()
        {
        }
    }

    [CollectionDefinition("Writing")]
    public class Writing_collection : ICollectionFixture<SqliteDatabaseFixture>
    {
        public Writing_collection() : base()
        {
        }
    }
}
