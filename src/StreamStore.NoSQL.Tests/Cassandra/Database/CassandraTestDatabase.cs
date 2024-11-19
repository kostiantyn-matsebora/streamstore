using Cassandra;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.Testing.Framework;

namespace StreamStore.NoSql.Tests.Cassandra.Database
{
    public sealed class CassandraTestDatabase : ITestDatabase
    {
        readonly Cluster cluster;
        readonly CassandraStorageConfiguration config;
        public readonly string Keyspace;
        private bool disposedValue;

        public CassandraTestDatabase(string keyspace, Action<Builder>? configureCluster = null)
        {
            Keyspace = keyspace;
            config = new CassandraStorageConfigurationBuilder().WithKeyspaceName(keyspace).Build();
            var configurator = configureCluster ?? ConfigureCluster;
            var builder = Cluster.Builder();
            configurator(builder);
            cluster = builder.Build();
        }

        public bool EnsureExists()
        {
            try
            {
                using (var session = cluster.Connect())
                {
                    session.Execute(
                        @$"CREATE KEYSPACE {config.Keyspace}
                              WITH REPLICATION = {{ 
                               'class' : 'SimpleStrategy', 
                               'replication_factor' : 1 
                              }};");
                    return true;
                }
            }
            catch
            {
                // ignored
                return false;
            }
        }

        void ConfigureCluster(Builder builder)
        {
            builder.AddContactPoint("localhost").WithQueryTimeout(10000);
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        using (var session = cluster.Connect())
                        {
                            session.Execute($"DROP KEYSPACE IF  EXISTS {config.Keyspace} ;");
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
