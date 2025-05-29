using Npgsql;
using StreamStore.Sql.Tests.Storage;

namespace StreamStore.Sql.Tests.PostgreSql.Storage
{
    public sealed class PostgresTestStorage: ISqlTestStorage
    {
        const string defaultConnectionString = "Host=localhost;Port=5432;Username=streamstore;Password=streamstore";
        readonly string serverConnectionString;
        readonly string databaseName;
        private bool disposedValue;

        public string ConnectionString { get; }

        public PostgresTestStorage(string databaseName, string? serverConnectionString = null)
        {
            this.databaseName = databaseName.ThrowIfNull(nameof(databaseName));
            this.serverConnectionString = serverConnectionString ?? defaultConnectionString;

            var connectionBuilder = new NpgsqlConnectionStringBuilder(this.serverConnectionString);
            connectionBuilder.Database = databaseName;
            ConnectionString =  connectionBuilder.ToString();
        }

        public bool EnsureExists()
        {
            try
            {
                using (var connection = new NpgsqlConnection(serverConnectionString))
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
                if (ex.SqlState == "42P04") // storage already exists
                {
                    return true;
                }
                return false;
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        using (var connection = new NpgsqlConnection(serverConnectionString))
                        {
                            connection.Open();
                            using (var command = connection.CreateCommand())
                            {
                                command.CommandText = $"DROP DATABASE {databaseName};";
                                command.ExecuteNonQuery();
                            }
                        }
                    } catch (Npgsql.NpgsqlException)
                    {
                        // ignored
                    }
                }

                disposedValue = true;
            }
        }


        public void Dispose()
        {

            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
