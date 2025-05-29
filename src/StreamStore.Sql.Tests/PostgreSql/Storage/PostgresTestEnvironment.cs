using StreamStore.Sql.Tests.Storage;


namespace StreamStore.Sql.Tests.PostgreSql.Storage
{
    public class PostgresTestEnvironment : SqlTestEnvironmentBase<PostgresTestStorage>
    {
        public PostgresTestEnvironment() : this(new PostgresStorageFixture())
        {
        }

        public PostgresTestEnvironment(PostgresStorageFixture fixture) : base(fixture)
        {
        }
    }
}
