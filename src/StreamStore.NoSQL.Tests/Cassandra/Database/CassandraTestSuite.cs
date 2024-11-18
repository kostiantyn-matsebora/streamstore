using StreamStore.Testing.StreamDatabase;

namespace StreamStore.NoSql.Tests.Cassandra.Database
{
    public class CassandraTestSuite : DatabaseSuiteBase
    {
        readonly CassandraDatabaseFixture fixture;

        public CassandraTestSuite() : this(new CassandraDatabaseFixture())
        {
        }

        public CassandraTestSuite(CassandraDatabaseFixture fixture)
        {
            this.fixture = fixture.ThrowIfNull(nameof(fixture));
        }

        protected override void ConfigureDatabase(ISingleTenantConfigurator configurator)
        {
            fixture.ConfigureDatabase(configurator);
        }
    }
}
