using System;
using Cassandra;
using StreamStore.Extensions;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;


namespace StreamStore.NoSql.Cassandra.Storage
{
    internal sealed class CassandraSessionFactory : ICassandraSessionFactory, IDisposable
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
            if (session.IsValueCreated)
            {
                session.Value.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        static ISession CreateSession(ICluster cluster, string keyspace)
        {
            return cluster.Connect(keyspace);
        }
    }
}
