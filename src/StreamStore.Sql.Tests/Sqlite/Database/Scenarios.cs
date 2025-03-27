using StreamStore.Testing.StreamDatabase.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.Sql.Tests.Sqlite.Database
{
    [Collection("Reading Sqlite")]
    public class Getting_actual_revision : Get_actual_revision<SqliteTestEnvironment>
    {
        public Getting_actual_revision(SqliteDatabaseFixture fixture) : base(new SqliteTestEnvironment(fixture))
        {
        }
    }

    [Collection("Reading Sqlite")]

    public class Reading_from_database : Reading_from_database<SqliteTestEnvironment>
    {
        public Reading_from_database(SqliteDatabaseFixture fixture, ITestOutputHelper output) : base(new SqliteTestEnvironment(fixture), output)
        {
        }
    }


    [Collection("Deleting Sqlite")]
    public class Deleting_from_database : Deleting_from_database<SqliteTestEnvironment>
    {
        public Deleting_from_database(SqliteDatabaseFixture fixture) : base(new SqliteTestEnvironment(fixture))
        {
        }
    }

    [Collection("Writing Sqlite")]
    public class Writing_to_database : Writing_to_database<SqliteTestEnvironment>
    {
        public Writing_to_database(SqliteDatabaseFixture fixture) : base(new SqliteTestEnvironment(fixture))
        {
        }
    }
}
