using StreamStore.Sql.Tests.Database;


namespace StreamStore.Sql.Tests.PostgreSql.Database
{
    public class PostgresTestEnvironment : SqlTestEnvironmentBase<PostgresTestDatabase>
    {
        public PostgresTestEnvironment() : this(new PostgresDatabaseFixture())
        {
        }

        public PostgresTestEnvironment(PostgresDatabaseFixture fixture) : base(fixture)
        {
        }
    }
}
