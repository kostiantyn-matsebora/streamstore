using Cassandra;
using StreamStore.Extensions;
using StreamStore.Testing.Framework;

namespace StreamStore.NoSql.Tests.Cassandra.Storage
{
    public sealed class CassandraTestStorage : ITestStorage
    {
        readonly Action<Builder> configureCluster;
        public readonly KeyspaceConfiguration Keyspace;
        bool disposedValue;
        Cluster? cluster;

        public CassandraTestStorage(KeyspaceConfiguration keyspace, Action<Builder> configureCluster)
        {
            Keyspace = keyspace;
            this.configureCluster  = configureCluster;
        }

        public bool EnsureExists()
        {
            try
            {
                var builder = Cluster.Builder();
                configureCluster(builder);
                cluster = builder.Build();

                using (var session = cluster.Connect())
                {
                   session.ExecuteAsync(new SimpleStatement(
                        @$"CREATE KEYSPACE {Keyspace.Name}
                              WITH REPLICATION = {{ 
                               'class' : '{Keyspace.ReplicationClass}', 
                               'replication_factor' : {Keyspace.ReplicationFactor}
                              }};")).GetAwaiter().GetResult();
                    return true;
                }
            }
            catch (Exception ex)
            {
                // ignored
                return false;
            }
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && cluster != null)
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
