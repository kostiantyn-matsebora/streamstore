
using Cassandra.Mapping;
using FluentAssertions;
using Moq;
using StreamStore.Exceptions;
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
            var uow = new CassandraStreamUnitOfWork(Generated.Id, Generated.Revision, null, Suite.StreamRepositoryFactory.Object);

            Suite.StreamRepository.Setup(x => x.AppendToStream(Generated.AnyArg<Id>())).ReturnsAsync(new AppliedInfo<EventEntity>(true));


            // Act
            var act = async () => await uow.SaveChangesAsync(CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_batch_fails()
        {
            // Arrange
            var uow = new CassandraStreamUnitOfWork(Generated.Id, Generated.Revision, null, Suite.StreamRepositoryFactory.Object);

            Suite.StreamRepository.Setup(x => x.AppendToStream(Generated.AnyArg<Id>())).ReturnsAsync(new AppliedInfo<EventEntity>(false));

            Suite.StreamRepository.Setup(x => x.GetStreamActualRevision(Generated.AnyArg<Id>())).ReturnsAsync(10);
            // Act
            var act = async () => await uow.SaveChangesAsync(CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<OptimisticConcurrencyException>();
        }
    }
}
