using StreamStore.Sql.PostgreSql;
using StreamStore.Sql.Tests.Database;


namespace StreamStore.Sql.Tests.PostgreSql.Database
{
    public sealed class PostgresDatabaseFixture : SqlDatabaseFixtureBase
    {
        public PostgresDatabaseFixture() : base()
        {
        }

        public override void ConfigureDatabase(IStreamStoreConfigurator configurator)
        {
            configurator.UsePostgresDatabase(c => c.WithConnectionString(GetConnectionString()));
        }

        protected override bool CreateDatabase(out string databaseName)
        {
            databaseName = Generated.DatabaseName;
            try
            {
                TryCreateDatabase(databaseName);
                return true;
            }
            catch (Npgsql.NpgsqlException)
            {
                return false;
            }
        }

        static void TryCreateDatabase(string databaseName)
        {
            var connectionString = $"Host=localhost;Port=5432;Username=streamstore;Password=streamstore";
            using (var connection = new Npgsql.NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"CREATE DATABASE {databaseName};";
                    command.ExecuteNonQuery();
                }
            }
        }

        string GetConnectionString()
        {
            return $"Host=localhost;Port=5432;Database={databaseName};Username=streamstore;Password=streamstore";
        }
    }
}
