using System;
using Cassandra;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;


namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraSessionFactory : ICassandraSessionFactory, IDisposable
    {
        readonly Lazy<ISession> session;
        public CassandraSessionFactory(ICluster cluster, CassandraStorageConfiguration config)
        {
            cluster.ThrowIfNull(nameof(cluster));
            config.ThrowIfNull(nameof(config));
            session = new Lazy<ISession>(() => CreateSession(cluster, config.Keyspace));
        }

        public ISession Open()
        {
            return session.Value;
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (disposing && session.IsValueCreated)
            {
               session.Value.Dispose();
            }
        }

        static ISession CreateSession(ICluster cluster, string keyspace)
        {
            return cluster.Connect(keyspace);
        }
    }
}
