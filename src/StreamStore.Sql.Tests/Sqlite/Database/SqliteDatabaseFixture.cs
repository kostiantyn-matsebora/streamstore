using System.Data.SQLite;
using StreamStore.Sql.Sqlite;
using StreamStore.SQL.Sqlite;
using StreamStore.Testing;


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
            var commandFactory =  new SqliteDapperCommandFactory(configuration);
            var exceptionHandler = new SqliteExceptionHandler();
            var provisioner = new SqlSchemaProvisioner(CreateConfiguration(), connectionFactory, commandFactory);
            await provisioner.ProvisionSchemaAsync(CancellationToken.None);

            var database = new SqlStreamDatabase(connectionFactory, commandFactory, exceptionHandler);
            Container.CopyTo(database);
        }


        SqlDatabaseConfiguration CreateConfiguration()
        {
            return
            new SqlDatabaseConfigurationBuilder()
                 .WithConnectionString($"Data Source ={databaseName}; Version = 3;")
                 .Build();
        }

        public void Dispose()
        {
        }
    }
}
