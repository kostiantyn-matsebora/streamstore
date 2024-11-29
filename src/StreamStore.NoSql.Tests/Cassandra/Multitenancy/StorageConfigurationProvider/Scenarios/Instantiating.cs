using FluentAssertions;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Multitenancy.StorageConfigurationProvider
{
    public class Instantiating: Scenario
    {
        [Fact]
        public void When_any_of_parameters_is_not_set()
        {

            // Act
            var act = () => new CassandraStorageConfigurationProvider(null!, new CassandraStorageConfiguration());

            // Assert
            act.Should().Throw<ArgumentNullException>();

            // Act
            act = () => new CassandraStorageConfigurationProvider(Generated.MockOf<ICassandraKeyspaceProvider>().Object, null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
