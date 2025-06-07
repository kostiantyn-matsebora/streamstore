using StreamStore.Testing.StreamStorage.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.Sql.Tests.Sqlite.Storage
{
    [Collection("Reading Sqlite")]
    public class Getting_stream_metadata : Getting_stream_metadata<SqliteTestEnvironment>
    {
        public Getting_stream_metadata(SqliteStorageFixture fixture) : base(new SqliteTestEnvironment(fixture))
        {
        }
    }

    [Collection("Reading Sqlite")]

    public class Reading_from_storage : Reading_from_storage<SqliteTestEnvironment>
    {
        public Reading_from_storage(SqliteStorageFixture fixture, ITestOutputHelper output) : base(new SqliteTestEnvironment(fixture), output)
        {
        }
    }


    [Collection("Deleting Sqlite")]
    public class Deleting_from_storage : Deleting_from_storage<SqliteTestEnvironment>
    {
        public Deleting_from_storage(SqliteStorageFixture fixture) : base(new SqliteTestEnvironment(fixture))
        {
        }
    }

    [Collection("Writing Sqlite")]
    public class Writing_to_storage : Writing_to_storage<SqliteTestEnvironment>
    {
        public Writing_to_storage(SqliteStorageFixture fixture) : base(new SqliteTestEnvironment(fixture))
        {
        }
    }
}
