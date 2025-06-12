using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.PostgreSql;
using StreamStore.Sql.Tests.PostgreSql.Storage;

namespace StreamStore.EventFlow.Tests
{
    [TestFixture]
    [Ignore("Requires Postgres server running")]
    public class Postgres_storage_tests : TestSuiteForEventStore
    {
        protected override void ConfigureStreamStorage(IServiceCollection services, string storageName)
        {
            services.UsePostgreSql(c => c.WithConnectionString($"Host=localhost;Port=5432;Username=streamstore;Password=streamstore;Database={storageName}"));
        }

        protected override void ProvisionStorage(string name)
        {
            new PostgresTestStorage(name).EnsureExists();
        }
    }
}
