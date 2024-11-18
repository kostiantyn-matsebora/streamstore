using StreamStore.Sql.Tests.Database;

namespace StreamStore.Sql.Tests.PostgreSql.Database
{
    public class PostgresTestDatabase: ISqlTestDatabase
    {
        const string defaultConnectionString = "Host=localhost;Port=5432;Username=streamstore;Password=streamstore";
        readonly string serverConnectionString;
        readonly string databaseName;

        public string ConnectionString { get; }

        public PostgresTestDatabase(string databaseName, string? serverConnectionString = null)
        {
            this.databaseName = databaseName.ThrowIfNull(nameof(databaseName));
            this.serverConnectionString = serverConnectionString ?? defaultConnectionString;

            var connectionBuilder = new Npgsql.NpgsqlConnectionStringBuilder(this.serverConnectionString);
            connectionBuilder.Database = databaseName;
            ConnectionString =  connectionBuilder.ToString();
        }

        public bool EnsureExists()
        {
            try
            {
                using (var connection = new Npgsql.NpgsqlConnection(serverConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = $"CREATE DATABASE {databaseName};";
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Npgsql.NpgsqlException ex)
            {
                if (ex.SqlState == "42P04")
                {
                    return true;
                }
                return false;
            }
        }
    }
}
