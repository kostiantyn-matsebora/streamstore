
using Cassandra;
using Cassandra.Data.Linq;
using Cassandra.Mapping;
using FluentAssertions;
using Moq;
using StreamStore.Exceptions;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Models;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Database.Mocking.UnitOfWork
{
    public class Saving_changes : Scenario<CassandraMockTestSuite>
    {

        [Fact]
       public async Task  When_batch_successfully_commited()
        {
            // Arrange
            var queries = new CassandraCqlQueries(new CassandraStorageConfiguration());
            var configure = new CassandraStatementConfigurator(new CassandraStorageConfiguration());
            var batch = MockBatch();
            var uow = Suite.StreamUnitOfWork;

            Suite.Mapper.Setup(x => x.CreateBatch(It.IsAny<BatchType>())).Returns(batch.Object);
            Suite.Mapper.Setup(x => x.ExecuteConditionalAsync<EventEntity>(It.IsAny<ICqlBatch>())).ReturnsAsync(new AppliedInfo<EventEntity>(true));


            // Act
            await uow.SaveChangesAsync(CancellationToken.None);

            // Assert
            Suite.MockRepository.VerifyAll();
        }

        [Fact]
        public async Task When_batch_fails()
        {
            // Arrange
            var queries = new CassandraCqlQueries(new CassandraStorageConfiguration());
            var configure = new CassandraStatementConfigurator(new CassandraStorageConfiguration());
            var uow = Suite.StreamUnitOfWork;
            var batch = MockBatch();

            Suite.Mapper.Setup(x => x.CreateBatch(BatchType.Logged)).Returns(batch.Object);
            Suite.Mapper.Setup(x => x.ExecuteConditionalAsync<EventEntity>(It.IsAny<ICqlBatch>())).ReturnsAsync(new AppliedInfo<EventEntity>(false));
            Suite.Mapper.Setup(x => x.SingleAsync<int?>(It.IsAny<Cql>())).ReturnsAsync(10);

            // Act
            var act = async () => await uow.SaveChangesAsync(CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<OptimisticConcurrencyException>();
            Suite.MockRepository.VerifyAll();
        }

        Mock<ICqlBatch> MockBatch()
        {
            var batch = Suite.MockRepository.Create<ICqlBatch>();
            batch.Setup(x => x.WithOptions(It.IsAny<Action<CqlQueryOptions>>())).Returns(batch.Object);
            return batch;
        }
    }
}
