using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Sqlite;
using StreamStore.Sql.Tests.Sqlite.Storage;

namespace StreamStore.EventFlow.Tests
{
    [TestFixture]
    public class Sqlite_storage_tests : TestSuiteForEventStore
    {
        protected override void ConfigureStreamStorage(IServiceCollection services, string storageName)
        {
            services.UseSqlite(c =>
                c.WithConnectionString($"Data Source = {storageName}; Version = 3;"));
        }

        protected override void ProvisionStorage(string name)
        {
           new SqliteTestStorage(name).EnsureExists();
        }
    }
}
