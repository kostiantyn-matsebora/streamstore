using System.Data.SQLite;
using Dapper.Extensions;
using Dapper.Extensions.Factory;
using Dapper.Extensions.SQLite;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.SQL.Sqlite;
using StreamStore.Testing;

namespace StreamStore.Sql.Tests.Sqlite
{
    public class SqliteTestSuite : ITestSuite
    {
        SqliteDatabaseConfiguration? configuration;
        readonly static object locker = new object();
        static bool databaseConfigured = false;

        public SqliteDatabaseConfiguration? Configuration => configuration;

        public bool IsReady => true;

        public async Task WithDatabase(Func<IStreamDatabase, Task> action)
        {
            await DapperFactory.StepAsync(async dapper =>
            {
                var database = new SqliteStreamDatabase(dapper, configuration!);
                await action(database);
            });
        }

        public void Initialize()
        {
            configuration = CreateSqliteConfiguration();

            if (!databaseConfigured)
            {
                lock (locker)
                {
                    if (!databaseConfigured)
                    {
                        ConfigureDapperFactory();
                        ProvisionSqliteSchema();
                        databaseConfigured = true;
                    }
                }

            }
        }

        void ConfigureDapperFactory()
        {
            DapperFactory.CreateInstance()
                .ConfigureServices(service =>
                {
                    service.AddDapperForSQLite();
                    service.AddDapperConnectionStringProvider<SqliteDapperConnectionStringProvider>();
                    service.AddSingleton(configuration!);
                }).Build();
        }

        SqliteDatabaseConfiguration CreateSqliteConfiguration()
        {
            return new SqliteDatabaseConfigurationBuilder()
              .WithConnectionString("Data Source=StreamStore.sqlite;Version=3;")
              .Build();
        }

        void ProvisionSqliteSchema()
        {
            SQLiteConnection.CreateFile("StreamStore.sqlite");
            DapperFactory.Step(dapper =>
            {
                var provisioner = new SqliteSchemaProvisioner(configuration!, dapper);
                provisioner.ProvisionSchemaAsync(CancellationToken.None).Wait();
            });
        }

      
    }
}
