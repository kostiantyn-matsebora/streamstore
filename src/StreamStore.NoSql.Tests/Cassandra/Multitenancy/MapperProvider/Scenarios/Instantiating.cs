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
        var act = () => new CassandraTenantMapperProvider(null!, Generated.Mocks.Single<ICassandraTenantClusterRegistry>().Object, Generated.Mocks.Single<ICassandraTenantMappingRegistry>().Object);

        // Assert
        act.Should().Throw<ArgumentNullException>();

        // Act
        act = () => new CassandraTenantMapperProvider(Generated.Mocks.Single<ICassandraTenantStorageConfigurationProvider>(). Object,  null!, Generated.Mocks.Single<ICassandraTenantMappingRegistry>().Object);

        // Assert
        act.Should().Throw<ArgumentNullException>();

                act = () => new CassandraTenantMapperProvider(Generated.Mocks.Single<ICassandraTenantStorageConfigurationProvider>(). Object,  Generated.Mocks.Single<ICassandraTenantClusterRegistry>().Object, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
