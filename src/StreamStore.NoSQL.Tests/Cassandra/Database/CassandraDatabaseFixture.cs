using StreamStore.Testing;
using StreamStore.Testing.Framework;
using StreamStore.NoSql.Cassandra;

namespace StreamStore.NoSql.Tests.Cassandra.Database
{
    public class CassandraDatabaseFixture : DatabaseFixtureBase<CassandraTestDatabase>
    {
        public CassandraDatabaseFixture() : this(new CassandraTestDatabase(Generated.DatabaseName))
        {
        }

        CassandraDatabaseFixture(CassandraTestDatabase database) : base(database)
        {

        }

        public override void ConfigureDatabase(ISingleTenantConfigurator configurator)
        {
            configurator.UseCassandra(c =>
                c.ConfigureCluster(x =>
                       x.AddContactPoint("localhost"))
                        .ConfigureKeyspace(k => k.WithKeyspaceName(testDatabase.Keyspace)));
        }

        protected override MemoryDatabase CreateContainer()
        {
            return new MemoryDatabase(new MemoryDatabaseOptions { Capacity = 1, EventPerStream = 5 });
        }
    }
}
