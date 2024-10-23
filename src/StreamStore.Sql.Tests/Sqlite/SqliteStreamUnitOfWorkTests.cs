using FluentAssertions;
using StreamStore.Exceptions;
using StreamStore.Testing;
using StreamStore.Testing.Framework;

namespace StreamStore.Sql.Tests.Sqlite
{
    public class SqliteStreamUnitOfWorkTests : StreamUnitOfWorkTestsBase<SqliteTestSuite>
    {
        public SqliteStreamUnitOfWorkTests() : base(new SqliteTestSuite())
        {
        }

        [Fact]
        public async Task SaveChangesAsync_Should_ThrowOptimisticConcurrencyWhenRevisionIsStale()
        {
            TrySkip();

            // Arrange
            var streamId = Generated.String;

            // Act
            var act = () =>
                    database!
                    .BeginAppendAsync(streamId)
                    .AddRangeAsync(Generated.CreateEventItems(3))
                    .SaveChangesAsync(CancellationToken.None);

            // Assert
            await act.Should().NotThrowAsync();

            act = () =>
                    database!
                    .BeginAppendAsync(streamId)
                    .AddRangeAsync(Generated.CreateEventItems(3))
                    .SaveChangesAsync(CancellationToken.None);

            await act.Should().ThrowAsync<OptimisticConcurrencyException>();
        }
    }
}
