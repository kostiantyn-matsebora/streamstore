using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Sqlite;
using StreamStore.Testing;

namespace StreamStore.Sql.Tests.DependencyConfigurator { 
    public class Configuring_dependencies: Scenario { 


        SqlDatabaseConfigurator CreateConfigurator(ServiceCollection? services = null)
        {
            var serviceCollection = services ?? new ServiceCollection();
            return new SqlDatabaseConfigurator(serviceCollection, new SqlDatabaseConfiguration());
        }

        [Fact]
        public void When_connection_factory_is_not_set()
        {
            // Arrange
            var configurator = CreateConfigurator();
            configurator.WithProvisioingQueryProvider<SqliteProvisioningQueryProvider>();

            // Act
            Action act = () => configurator.Configure(false);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void When_provisioning_query_provider_is_not_set()
        {
            // Arrange
            var configurator = CreateConfigurator();
            configurator.WithConnectionFactory<SqliteDbConnectionFactory>();

            // Act
            Action act = () => configurator.Configure(false);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void When_required_dependencies_are_set()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();
            var configurator = CreateConfigurator(serviceCollection);
            configurator.WithConnectionFactory<SqliteDbConnectionFactory>();
            configurator.WithProvisioingQueryProvider<SqliteProvisioningQueryProvider>();
            configurator.WithExceptionHandling<SqliteExceptionHandler>();


            // Act
            configurator.Configure(false);
            
            // Assert
            serviceCollection.Should().NotBeEmpty();
            serviceCollection.Should().Contain(x => x.ServiceType == typeof(IDapperCommandFactory));
            serviceCollection.Should().Contain(x => x.ServiceType == typeof(ISqlQueryProvider));
            serviceCollection.Should().Contain(x => x.ServiceType == typeof(IDbConnectionFactory) && x.ImplementationType == typeof(SqliteDbConnectionFactory));
            serviceCollection.Should().Contain(x => x.ServiceType == typeof(ISqlProvisioningQueryProvider) && x.ImplementationType == typeof(SqliteProvisioningQueryProvider));
            serviceCollection.Should().Contain(x => x.ServiceType == typeof(ISqlExceptionHandler) && x.ImplementationType == typeof(SqliteExceptionHandler));
        }
    }


}
