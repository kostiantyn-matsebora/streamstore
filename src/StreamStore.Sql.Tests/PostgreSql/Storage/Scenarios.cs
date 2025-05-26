using StreamStore.Testing.StreamStorage.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.Sql.Tests.PostgreSql.Storage
{
    [Collection("Reading Postgres")]
    public class Getting_stream_metadata : Getting_stream_metadata<PostgresTestEnvironment>
    {
        public Getting_stream_metadata(PostgresStorageFixture fixture) : base(new PostgresTestEnvironment(fixture))
        {
        }
    }

    [Collection("Reading Postgres")]

    public class Reading_from_storage : Reading_from_storage<PostgresTestEnvironment>
    {
        public Reading_from_storage(PostgresStorageFixture fixture, ITestOutputHelper output) : base(new PostgresTestEnvironment(fixture), output)
        {
        }
    }


   [Collection("Deleting Postgres")]
    public class Deleting_from_storage : Deleting_from_storage<PostgresTestEnvironment>
    {
        public Deleting_from_storage(PostgresStorageFixture fixture) : base(new PostgresTestEnvironment(fixture))
        {
        }
    }

   [Collection("Writing Postgres")]
    public class Writing_to_storage : Writing_to_storage<PostgresTestEnvironment>
    {
        public Writing_to_storage(PostgresStorageFixture fixture) : base(new PostgresTestEnvironment(fixture))
        {
        }
    }
}
