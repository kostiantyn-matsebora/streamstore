using StreamStore.Testing.StreamDatabase.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.Sql.Tests.PostgreSql.Database
{
    [Collection("Reading Postgres")]
    public class Getting_actual_revision : Get_actual_revision<PostgresTestEnvironment>
    {
        public Getting_actual_revision(PostgresDatabaseFixture fixture) : base(new PostgresTestEnvironment(fixture))
        {
        }
    }

    [Collection("Reading Postgres")]

    public class Reading_from_database : Reading_from_database<PostgresTestEnvironment>
    {
        public Reading_from_database(PostgresDatabaseFixture fixture, ITestOutputHelper output) : base(new PostgresTestEnvironment(fixture), output)
        {
        }
    }


   [Collection("Deleting Postgres")]
    public class Deleting_from_database : Deleting_from_database<PostgresTestEnvironment>
    {
        public Deleting_from_database(PostgresDatabaseFixture fixture) : base(new PostgresTestEnvironment(fixture))
        {
        }
    }

   [Collection("Writing Postgres")]
    public class Writing_to_database : Writing_to_database<PostgresTestEnvironment>
    {
        public Writing_to_database(PostgresDatabaseFixture fixture) : base(new PostgresTestEnvironment(fixture))
        {
        }
    }
}
