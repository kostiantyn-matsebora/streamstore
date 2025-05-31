using Cassandra;
using Cassandra.Mapping;
using FluentAssertions;
using Moq;
using StreamStore.Exceptions.Appending;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Models;
using StreamStore.NoSql.Cassandra.Storage;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Storage.Mocking.Scenarios
{
    public class Saving_changes : Scenario<CassandraMockTestEnvironment>
    {

        [Fact]
       public async Task  When_batch_successfully_commited()
        {
            // Arrange
            var queries = new CassandraCqlQueries(new CassandraStorageConfiguration());
            var configure = new CassandraStatementConfigurator(new CassandraStorageConfiguration());
            var batch = MockBatch();
            var writer = Environment.StreamStorage;
            var streamId = Generated.Primitives.Id;

            Environment.Mapper.Setup(x => x.CreateBatch(It.IsAny<BatchType>())).Returns(batch.Object);
            Environment.Mapper.Setup(x => x.ExecuteConditionalAsync<EventEntity>(It.IsAny<ICqlBatch>())).ReturnsAsync(new AppliedInfo<EventEntity>(true));


            // Act
            await writer.WriteAsync(streamId, Enumerable.Empty<IStreamEventRecord>(), CancellationToken.None);


            // Assert
            Environment.MockRepository.VerifyAll();
        }

        [Fact]
        public async Task When_batch_fails()
        {
            // Arrange
            var queries = new CassandraCqlQueries(new CassandraStorageConfiguration());
            var configure = new CassandraStatementConfigurator(new CassandraStorageConfiguration());
            var writer = Environment.StreamStorage;
            var batch = MockBatch();
            var streamId = Generated.Primitives.Id;

            Environment.Mapper.Setup(x => x.CreateBatch(BatchType.Logged)).Returns(batch.Object);
            Environment.Mapper.Setup(x => x.ExecuteConditionalAsync<EventEntity>(It.IsAny<ICqlBatch>())).ReturnsAsync(new AppliedInfo<EventEntity>(false) { Existing = new EventEntity { Revision = 1 } });
            

            // Act
            var act = async () => await writer.WriteAsync(streamId, Enumerable.Empty<IStreamEventRecord>(), CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<RevisionAlreadyExistsException>();
            Environment.MockRepository.VerifyAll();
        }

        Mock<ICqlBatch> MockBatch()
        {
            var batch = Environment.MockRepository.Create<ICqlBatch>();
            batch.Setup(x => x.WithOptions(It.IsAny<Action<CqlQueryOptions>>())).Returns(batch.Object);
            return batch;
        }
    }
}
