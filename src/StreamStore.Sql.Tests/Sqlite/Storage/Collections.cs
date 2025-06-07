namespace StreamStore.Sql.Tests.Sqlite.Storage
{
    [CollectionDefinition("Reading Sqlite")]
    public class Reading_collection : ICollectionFixture<SqliteStorageFixture>
    {
        public Reading_collection() : base()
        {
        }
    }

    [CollectionDefinition("Deleting Sqlite")]
    public class Deleting_collection : ICollectionFixture<SqliteStorageFixture>
    {
        public Deleting_collection() : base()
        {
        }
    }

    [CollectionDefinition("Writing Sqlite")]
    public class Writing_collection : ICollectionFixture<SqliteStorageFixture>
    {
        public Writing_collection() : base()
        {
        }
    }
}
