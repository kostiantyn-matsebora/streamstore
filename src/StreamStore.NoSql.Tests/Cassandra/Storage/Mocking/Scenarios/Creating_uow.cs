using System.IO;
using Cassandra.Mapping;
using FluentAssertions;
using Moq;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Storage.Mocking
{
    public class Creating_uow: Scenario<CassandraMockTestEnvironment>
    {
        [Fact]
        public async Task When_begin_appending()
        {

            // Arrange 
            var streamId = Generated.Primitives.Id;
            Environment.Mapper.Setup(x => x.SingleOrDefaultAsync<int?>(It.IsAny<Cql>())).ReturnsAsync(0);
            Environment.Queries.Setup(x => x.StreamActualRevision(It.IsAny<string>())).Returns(new Cql(Generated.Primitives.String));

            // Act
            var uow = await Environment.StreamStorage.BeginAppendAsync(streamId);
            
            // Assert
            uow.Should().NotBeNull();
        }
    }
}
