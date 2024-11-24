using Cassandra;
using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Database.Repository
{
    public class Disposing: Scenario
    {
        [Fact]
        public void When_disposing()
        {
            // Arrange
            var sessionFactory = Suite.MockRepository.Create<ICassandraSessionFactory>();
            var session = Suite.MockRepository.Create<ISession>();
            session.Setup(x => x.Dispose());
            var mapperFactory = Suite.MockRepository.Create<ICassandraMapperFactory>();
            mapperFactory.Setup(x => x.CreateMapper(session.Object)).Returns(Suite.MockRepository.Create<IMapper>().Object);
            sessionFactory.Setup(x => x.Open()).Returns(session.Object);
            var repository = new CassandraStreamRepository(sessionFactory.Object, mapperFactory.Object, new CassandraStorageConfiguration());

            // Act
            repository.Dispose();

            // Assert
            Suite.MockRepository.VerifyAll();
        }
    }
}
