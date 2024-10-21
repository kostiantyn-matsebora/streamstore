using System.Data.SQLite;
using Dapper.Extensions;
using Dapper.Extensions.SQLite;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.SQL.Sqlite;
using StreamStore.Testing.Framework;


namespace StreamStore.Sql.Tests.Sqlite
{
    public class SqliteTestSuite : TestSuiteBase
    {
        protected override void RegisterServices(IServiceCollection services)
        {
            services.AddDapperForSQLite();
            services.AddSingleton(CreateSqliteConfiguration());
        }

        protected override async Task SetUp()
        {
            SQLiteConnection.CreateFile("StreamStore.sqlite");
            await ProvisionSchema();
        }

        private async Task ProvisionSchema()
        {
            using (var dapper = Services.GetRequiredService<IDapper>())
            {
                var provisioner = new SqliteSchemaProvisioner(CreateSqliteConfiguration(), dapper);
                await provisioner.ProvisionSchemaAsync(CancellationToken.None);
            }
        }

        static SqliteDatabaseConfiguration CreateSqliteConfiguration()
        {
            return new SqliteDatabaseConfigurationBuilder()
              .WithConnectionString("Data Source=StreamStore.sqlite;Version=3;")
              .Build();
        }
    }
}
