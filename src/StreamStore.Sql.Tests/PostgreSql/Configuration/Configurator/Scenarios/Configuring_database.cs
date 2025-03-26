using StreamStore.Testing;
using StreamStore.Sql.PostgreSql;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Configuration;
using FluentAssertions;
using StreamStore.Sql.API;
using StreamStore.Configuration.Database;

namespace StreamStore.Sql.Tests.PostgreSql.Configuration.Configurator
{
    public class Configuring_database : Scenario
    {
        [Fact]
        public void When_configuring_streamstore_database()
        {

            // Arrange
            var configurator = new SingleTenantConfigurator();
            var connectionString = Generated.Primitives.String;

            // Act

            var serviceCollection = configurator
               .UsePostgresDatabase(c => c.ConfigureDatabase(x => x.WithConnectionString(connectionString)))
               .Configure();

            var provider = serviceCollection.BuildServiceProvider();

            // Assert
            var configuration = provider.GetRequiredService<SqlDatabaseConfiguration>();
            configuration.ConnectionString.Should().Be(connectionString);
            configuration.SchemaName.Should().Be("public");
            configuration.TableName.Should().Be("Events");
            provider.GetService<IDbConnectionFactory>().Should().BeOfType<PostgresConnectionFactory>();
            provider.GetService<ISqlExceptionHandler>().Should().BeOfType<PostgresExceptionHandler>();
            provider.GetService<ISqlProvisioningQueryProvider>().Should().BeOfType<PostgresProvisioningQueryProvider>();
        }
    }
}
