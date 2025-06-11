using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.PostgreSql;
using StreamStore.Sql.Tests.Storage;
using StreamStore.Testing;


namespace StreamStore.Sql.Tests.PostgreSql.Storage
{
    public sealed class PostgresStorageFixture : SqlStorageFixtureBase<PostgresTestStorage>
    {

        public PostgresStorageFixture() : base(new PostgresTestStorage(Generated.Names.Storage))
        {
        }

        public override void ConfigurePersistence(IServiceCollection services)
        {
            services.UsePostgreSql(c => c.WithConnectionString(testStorage.ConnectionString));
        }
    }
}
