using System.IO;
using Cassandra.Mapping;
using FluentAssertions;
using Moq;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Database.Mocking
{
    public class Creating_uow: Scenario<CassandraMockTestSuite>
    {
        [Fact]
        public async Task When_begin_appending()
        {

            // Arrange 
            var streamId = Generated.Id;
            Suite.Mapper.Setup(x => x.SingleAsync<int?>(It.IsAny<Cql>())).ReturnsAsync(0);

            // Act
            var uow = await Suite.StreamDatabase.BeginAppendAsync(streamId);
            
            // Assert
            uow.Should().NotBeNull();
        }
    }
}
