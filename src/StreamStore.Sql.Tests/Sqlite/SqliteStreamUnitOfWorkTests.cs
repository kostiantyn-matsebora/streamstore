using FluentAssertions;
using StreamStore.Exceptions;
using StreamStore.Testing;

namespace StreamStore.Sql.Tests.Sqlite
{
    public class SqliteStreamUnitOfWorkTests : StreamUnitOfWorkTestsBase
    {
        public SqliteStreamUnitOfWorkTests() : base(new SqliteTestSuite())
        {
        }

        [Fact]
        public async Task SaveChangesAsync_Should_ThrowOptimisticConcurrencyWhenRevisionIsStale()
        {
            TrySkip();

            // Arrange
            var streamId = GeneratedValues.String;

            await suite.WithDatabase(async database =>
            {
                // Act
                var act = () =>
                        database!
                        .BeginAppendAsync(streamId)
                        .AddRangeAsync(GeneratedValues.CreateEventItems(3))
                        .SaveChangesAsync(CancellationToken.None);

                // Assert
                await act.Should().NotThrowAsync();

                act = () =>
                        database!
                        .BeginAppendAsync(streamId)
                        .AddRangeAsync(GeneratedValues.CreateEventItems(3))
                        .SaveChangesAsync(CancellationToken.None);
                await act.Should().ThrowAsync<OptimisticConcurrencyException>();
            });
        }
    }
}
