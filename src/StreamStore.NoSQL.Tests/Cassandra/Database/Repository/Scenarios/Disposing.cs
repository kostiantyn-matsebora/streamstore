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
            var config = new CassandraStorageConfiguration();

            var mapper = Suite.MockRepository.Create<ICassandraMapper>();
            mapper.Setup(x => x.Dispose());
            var repository = new CassandraStreamRepository(mapper.Object, config);

            // Act
            repository.Dispose();

            // Assert
            Suite.MockRepository.VerifyAll();
        }
    }
}
