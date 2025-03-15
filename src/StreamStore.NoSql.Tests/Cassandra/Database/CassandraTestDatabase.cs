using Cassandra;
using StreamStore.Testing.Framework;

namespace StreamStore.NoSql.Tests.Cassandra.Database
{
    public sealed class CassandraTestDatabase : ITestDatabase
    {
        readonly Cluster cluster;
        public readonly KeyspaceConfiguration Keyspace;
        bool disposedValue;

        public CassandraTestDatabase(KeyspaceConfiguration keyspace, Action<Builder> configureCluster)
        {
            Keyspace = keyspace;
            var builder = Cluster.Builder();
            configureCluster(builder);
            cluster = builder.Build();
        }

        public bool EnsureExists()
        {
            try
            {
                using (var session = cluster.Connect())
                {
                    session.Execute(
                        @$"CREATE KEYSPACE {Keyspace.Name}
                              WITH REPLICATION = {{ 
                               'class' : '{Keyspace.ReplicationClass}', 
                               'replication_factor' : {Keyspace.ReplicationFactor}
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
                            session.Execute($"DROP KEYSPACE IF  EXISTS {Keyspace.Name} ;");
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

    public class KeyspaceConfiguration
    {
        public KeyspaceConfiguration(string name)
        {
            this.Name = name.ThrowIfNull(nameof(name));
        }
        public string Name { get; }
        public int ReplicationFactor { get; set; } = 1;
        public string ReplicationClass { get; set; } = "SimpleStrategy";

    }
}
