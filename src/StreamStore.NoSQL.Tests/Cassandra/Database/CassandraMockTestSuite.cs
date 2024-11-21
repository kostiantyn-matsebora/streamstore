using Moq;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Testing.Framework;

namespace StreamStore.NoSql.Tests.Cassandra.Database
{
    public class CassandraMockTestSuite: TestSuite
    {

        internal readonly Mock<ICassandraStreamRepositoryFactory> StreamRepositoryFactory;
        internal readonly Mock<ICassandraStreamRepository> StreamRepository;
        internal readonly CassandraStreamDatabase StreamDatabase;

        public CassandraMockTestSuite()
        {
            StreamRepositoryFactory = MockRepository.Create<ICassandraStreamRepositoryFactory>();
            StreamRepository = MockRepository.Create<ICassandraStreamRepository>();
            StreamRepository.Setup(x => x.Dispose());
            StreamRepositoryFactory.Setup(x => x.Create()).Returns(StreamRepository.Object);
            StreamDatabase = new CassandraStreamDatabase(StreamRepositoryFactory.Object);
        }
    }
}
