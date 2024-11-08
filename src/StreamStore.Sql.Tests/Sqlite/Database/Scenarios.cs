using StreamStore.Testing.StreamDatabase.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.Sql.Tests.Sqlite.Database
{
    [Collection("Reading")]
    public class Finding_stream_metadata : Find_stream_data<SqliteTestSuite>
    {
        public Finding_stream_metadata(SqliteDatabaseFixture fixture) : base(new SqliteTestSuite(fixture))
        {
        }
    }

    [Collection("Reading")]

    public class Reading_from_database : Reading_from_database<SqliteTestSuite>
    {
        public Reading_from_database(SqliteDatabaseFixture fixture, ITestOutputHelper output) : base(new SqliteTestSuite(fixture), output)
        {
        }
    }


    [Collection("Deleting")]
    public class Deleting_from_database : Deleting_from_database<SqliteTestSuite>
    {
        public Deleting_from_database(SqliteDatabaseFixture fixture) : base(new SqliteTestSuite(fixture))
        {
        }
    }

    [Collection("Writing")]
    public class Writing_to_database : Writing_to_database<SqliteTestSuite>
    {
        public Writing_to_database(SqliteDatabaseFixture fixture) : base(new SqliteTestSuite(fixture))
        {
        }
    }
}
