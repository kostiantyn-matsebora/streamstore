using StreamStore.Testing;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Configuration;
using FluentAssertions;
using StreamStore.Sql.API;
using StreamStore.Sql.Sqlite;

namespace StreamStore.Sql.Tests.Sqlite.Configurator
{
    public class Configuring_database: Scenario
    {
        [Fact]
        public void When_configuring_streamstore_database()
        {
            // Arrange
            var registrator = new SingleTenantDatabaseRegistrator();

            var connectionString = Generated.String;
            var serviceCollection = new ServiceCollection();

            // Act
            registrator
                .UseSqliteDatabase(c =>  c.ConfigureDatabase( x => x.WithConnectionString(connectionString)))
                .Apply(serviceCollection);

            var provider = serviceCollection.BuildServiceProvider();

            // Assert
            var configuration = provider.GetRequiredService<SqlDatabaseConfiguration>();
            configuration.ConnectionString.Should().Be(connectionString);
            configuration.SchemaName.Should().Be("main");
            configuration.TableName.Should().Be("Events");
            provider.GetService<IDbConnectionFactory>().Should().BeOfType<SqliteDbConnectionFactory>();
            provider.GetService<ISqlExceptionHandler>().Should().BeOfType<SqliteExceptionHandler>();
            provider.GetService<ISqlProvisioningQueryProvider>().Should().BeOfType<SqliteProvisioningQueryProvider>();
        }
    }
}
