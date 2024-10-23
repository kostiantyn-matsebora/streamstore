using System.Data.SQLite;
using Dapper.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Sqlite;
using StreamStore.SQL.Sqlite;
using StreamStore.Testing.StreamDatabase;


namespace StreamStore.Sql.Tests.Sqlite.Database
{
    public class SqliteTestSuite : DatabaseSuiteBase
    {
        protected override async Task SetUp()
        {
            SQLiteConnection.CreateFile("StreamStore.sqlite");
            await ProvisionSchema();
        }

        private async Task ProvisionSchema()
        {
            using (var dapper = Services.GetRequiredService<IDapper>())
            {
                var provisioner = new SqliteSchemaProvisioner(Services.GetRequiredService<SqliteDatabaseConfiguration>(), dapper);
                await provisioner.ProvisionSchemaAsync(CancellationToken.None);
            }
        }

        protected override void ConfigureDatabase(IStreamStoreConfigurator configurator)
        {
            configurator.UseSqliteDatabase(CreateConfiguration());
        }

        static IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "connectionStrings:StreamStore", "Data Source=StreamStore.sqlite;Version=3;" }
                }).Build();
        }
    }
}
