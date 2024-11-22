using FluentAssertions;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.NoSql.Cassandra.Provisioning;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Provisioning.ProvisionerFactory
{
    public class Instantiating: Scenario
    {

        [Fact]
        public void When_any_of_parameters_is_not_set()
        {

            // Act
            var act = () => new CassandraSchemaProvisionerFactory(null!, Generated.MockOf<ICassandraPredicateProvider>().Object, Generated.MockOf<ICassandraTenantClusterRegistry>().Object);

            // Assert
            act.Should().Throw<ArgumentNullException>();

            // Act
            act = () => new CassandraSchemaProvisionerFactory(Generated.MockOf<ICassandraStorageConfigurationProvider>().Object, null!, Generated.MockOf<ICassandraTenantClusterRegistry>().Object);

            // Assert
            act.Should().Throw<ArgumentNullException>();

            // Act
            act = () => new CassandraSchemaProvisionerFactory(Generated.MockOf<ICassandraStorageConfigurationProvider>().Object, Generated.MockOf<ICassandraPredicateProvider>().Object, null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
