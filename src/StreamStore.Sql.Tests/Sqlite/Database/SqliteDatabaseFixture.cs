using System.Data.SQLite;
using AutoFixture;
using StreamStore.SQL.Sqlite;
using StreamStore.Testing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StreamStore.Sql.Tests.Sqlite.Database
{
    public sealed class SqliteDatabaseFixture : IDisposable
    {
        readonly string databaseName = $"{Generated.String}.sqlite";

        public readonly MemoryDatabase Container = new MemoryDatabase();

        public string DatabaseName => databaseName;

        public SqliteDatabaseFixture()
        {
            SQLiteConnection.CreateFile($"{databaseName}");
            ProvisionDatabase().Wait();
        }


        async Task ProvisionDatabase()
        {

            var configuration = CreateConfiguration();
            var connectionFactory = new SqliteDbConnectionFactory(configuration);

            var provisioner = new SqliteSchemaProvisioner(CreateConfiguration(), connectionFactory);
            await provisioner.ProvisionSchemaAsync(CancellationToken.None);

            var database = new SqliteStreamDatabase(connectionFactory, configuration);
            await Container.CopyTo(database);
        }


        SqliteDatabaseConfiguration CreateConfiguration()
        {
            return
            new SqliteDatabaseConfigurationBuilder()
                 .WithConnectionString($"Data Source ={databaseName}; Version = 3;")
                 .Build();
        }

        public void Dispose()
        {
        }
    }


}
