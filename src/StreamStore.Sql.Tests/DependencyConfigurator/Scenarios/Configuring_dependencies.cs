using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Provisioning;
using StreamStore.Sql.Sqlite;
using StreamStore.Sql.Sqlite.Migrations;
using StreamStore.Testing;

namespace StreamStore.Sql.Tests.DependencyConfigurator
{
    public class Configuring_dependencies : Scenario
    {
        static SqlSingleTenantStorageConfigurator CreateConfigurator(ServiceCollection? services = null)
        {
            var serviceCollection = services ?? new ServiceCollection();
            return new SqlSingleTenantStorageConfigurator(serviceCollection, new SqlStorageConfiguration());
        }

        [Fact]
        public void When_connection_factory_is_not_set()
        {
            // Arrange
            var configurator = CreateConfigurator();

            // Act
            Action act = () => configurator.Apply();

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
            configurator.WithExceptionHandling<SqliteExceptionHandler>();
            configurator.WithMigrator<SqliteMigrator>();
            configurator.WithMigrationAssembly(typeof(SqliteMigrator).Assembly);

            // Act
            configurator.Apply();

            // Assert
            serviceCollection.Should().NotBeEmpty();
            serviceCollection.Should().Contain(x => x.ServiceType == typeof(IDapperCommandFactory));
            serviceCollection.Should().Contain(x => x.ServiceType == typeof(ISqlQueryProvider));
            serviceCollection.Should().Contain(x => x.ServiceType == typeof(IDbConnectionFactory) && x.ImplementationType == typeof(SqliteDbConnectionFactory));
            serviceCollection.Should().Contain(x => x.ServiceType == typeof(ISqlExceptionHandler) && x.ImplementationType == typeof(SqliteExceptionHandler));
            serviceCollection.Should().Contain(x => x.ServiceType == typeof(IMigrator) && x.ImplementationType == typeof(SqliteMigrator));
        }
    }


}
