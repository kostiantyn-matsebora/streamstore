using StreamStore.Testing;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Configuration;
using FluentAssertions;
using StreamStore.Sql.API;
using StreamStore.Sql.Sqlite;
using StreamStore.Configuration.Database;

namespace StreamStore.Sql.Tests.Sqlite.Configuration.Configurator
{
    public class Configuring_database : Scenario
    {
        [Fact]
        public void When_configuring_streamstore_database()
        {
            // Arrange
            var configurator = new SingleTenantConfigurator();
            var connectionString = Generated.String;


            // Act
            var serviceCollection = configurator
                    .UseSqliteDatabase(c => c.ConfigureDatabase(x => x.WithConnectionString(connectionString)))
                    .Configure();


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
