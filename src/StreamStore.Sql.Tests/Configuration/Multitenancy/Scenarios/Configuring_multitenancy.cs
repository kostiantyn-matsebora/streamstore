﻿using FluentAssertions;
using StreamStore.Testing;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Database;
using StreamStore.Provisioning;
using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;
using Microsoft.Extensions.Configuration;
using StreamStore.Multitenancy;
using StreamStore.Sql.Multitenancy;



namespace StreamStore.Sql.Tests.Configuration.MultiTenant
{
    public abstract class Configuring_multitenancy<TEnvironment> : Scenario<TEnvironment> where TEnvironment : MultitenantConfiguratorTestEnvironmentBase, new()
    {
        [Fact]
        public void When_connection_string_provider_is_not_set()
        {
            // Arrange
            var configurator = Environment.CreateSqlDatabaseConfigurator(new ServiceCollection());

            // Act
            var act = () => configurator.Apply();

            //Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Neither ISqlTenantConnectionStringProvider nor tenant connection strings provided.");
        }

        [Fact]
        public void When_multitenancy_is_configured()
        {
            // Arrange
            var services = new ServiceCollection();
            var configurator = new StreamStoreConfigurator();

            // Act
            configurator.WithMultitenancy(x => 
                    Environment.UseDatabase(x, c => 
                        c.ConfigureDatabase(x => 
                            x.WithConnectionString("connectionString")
                             .WithSchema("schema")
                             .WithTable("table"))));

            configurator.Configure(services);

            //Assert
            var provider = services.BuildServiceProvider();

            provider.GetRequiredService<ITenantStreamDatabaseProvider>()
                     .Should().NotBeNull()
                     .And.BeAssignableTo<SqlTenantStreamDatabaseProvider>();

            provider.GetRequiredService<ITenantSchemaProvisionerFactory>().Should().NotBeNull();
            provider.GetRequiredService<ISqlExceptionHandler>().Should().NotBeNull();
            provider.GetRequiredService<ISqlTenantConnectionStringProvider>().Should().NotBeNull();
            provider.GetRequiredService<ISqlTenantDatabaseConfigurationProvider>().Should().NotBeNull();

            var configuration = provider.GetRequiredService<SqlDatabaseConfiguration>();
            configuration.ConnectionString.Should().Be("connectionString");
            configuration.SchemaName.Should().Be("schema");
            configuration.TableName.Should().Be("table");
        }


        [Fact]
        public void When_multitenancy_is_configured_with_appsettings()
        {
            // Arrange
            var services = new ServiceCollection();
            var configurator = new StreamStoreConfigurator();
            var appSettings = new Dictionary<string, string?>
            {
                {"ConnectionStrings:StreamStore", "connectionString"},
                {$"{Environment.SectionName}:SchemaName", "schema"},
                {$"{Environment.SectionName}:TableName", "table"}
            };

            var config = new ConfigurationBuilder()
                    .AddInMemoryCollection(appSettings)
                    .Build();
            // Act
            configurator.WithMultitenancy(x => Environment.UseDatabaseWithAppSettings(x, config));

            configurator.Configure(services);

            //Assert
            var provider = services.BuildServiceProvider();

            provider.GetRequiredService<ITenantStreamDatabaseProvider>()
                     .Should().NotBeNull()
                     .And.BeAssignableTo<SqlTenantStreamDatabaseProvider>();

            provider.GetRequiredService<ITenantSchemaProvisionerFactory>().Should().NotBeNull();
            provider.GetRequiredService<ISqlExceptionHandler>().Should().NotBeNull();
            provider.GetRequiredService<ISqlTenantConnectionStringProvider>().Should().NotBeNull();
            provider.GetRequiredService<ISqlTenantDatabaseConfigurationProvider>().Should().NotBeNull();

            var configuration = provider.GetRequiredService<SqlDatabaseConfiguration>();
            configuration.ConnectionString.Should().Be("connectionString");
            configuration.SchemaName.Should().Be("schema");
            configuration.TableName.Should().Be("table");
        }
    }
}
