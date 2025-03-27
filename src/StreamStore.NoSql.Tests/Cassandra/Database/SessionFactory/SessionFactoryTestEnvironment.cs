using Cassandra;
using Moq;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Testing.Framework;

namespace StreamStore.NoSql.Tests.Cassandra.Database.SessionFactory
{
    public class SessionFactoryTestEnvironment: TestEnvironment
    {
        internal readonly Mock<ICluster> Cluster;
        internal readonly Mock<ISession> Session;
        internal readonly CassandraSessionFactory SessionFactory;

        public SessionFactoryTestEnvironment()
        {
            var configuration = new CassandraStorageConfiguration();

            Cluster = MockRepository.Create<ICluster>();
            Session = MockRepository.Create<ISession>();
            Cluster.Setup(c => c.Connect(configuration.Keyspace)).Returns(Session.Object);
            SessionFactory = new CassandraSessionFactory(Cluster.Object, configuration);
        }
    }
}
