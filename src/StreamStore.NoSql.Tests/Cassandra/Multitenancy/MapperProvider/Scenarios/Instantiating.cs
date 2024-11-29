using FluentAssertions;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Multitenancy.MapperProvider;

public class Instantiating: Scenario
{
    [Fact]
    public void When_any_argument_is_not_set()
    {
        // Act
        var act = () => new CassandraTenantMapperProvider(null!, Generated.MockOf<ICassandraTenantClusterRegistry>().Object, Generated.MockOf<ICassandraTenantMappingRegistry>().Object);

        // Assert
        act.Should().Throw<ArgumentNullException>();

        // Act
        act = () => new CassandraTenantMapperProvider(Generated.MockOf<ICassandraTenantStorageConfigurationProvider>(). Object,  null!, Generated.MockOf<ICassandraTenantMappingRegistry>().Object);

        // Assert
        act.Should().Throw<ArgumentNullException>();

                act = () => new CassandraTenantMapperProvider(Generated.MockOf<ICassandraTenantStorageConfigurationProvider>(). Object,  Generated.MockOf<ICassandraTenantClusterRegistry>().Object, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
