﻿using StreamStore.Testing;
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
            var configurator = new StreamStoreConfigurator();
            var connectionString = Generated.String;
            var serviceCollection = new ServiceCollection();

            // Act
            configurator.UseSqliteDatabase(c =>  c.ConfigureDatabase( x => x.WithConnectionString(connectionString)));
            var provider =configurator.Configure(serviceCollection).BuildServiceProvider();

            // Assert
            var configuration = provider.GetRequiredService<SqlDatabaseConfiguration>();
            configuration.ConnectionString.Should().Be(connectionString);
            configuration.SchemaName.Should().Be("main");
            configuration.TableName.Should().Be("Events");
            configuration.ProvisionSchema.Should().BeTrue();
            provider.GetService<IDbConnectionFactory>().Should().BeOfType<SqliteDbConnectionFactory>();
            provider.GetService<ISqlExceptionHandler>().Should().BeOfType<SqliteExceptionHandler>();
            provider.GetService<ISqlProvisioningQueryProvider>().Should().BeOfType<SqliteProvisioningQueryProvider>();
        }
    }
}
