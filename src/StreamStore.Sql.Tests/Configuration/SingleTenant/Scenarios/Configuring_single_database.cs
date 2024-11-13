using FluentAssertions;
using StreamStore.Testing;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Database;
using StreamStore.Provisioning;
using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;
using Microsoft.Extensions.Configuration;



namespace StreamStore.Sql.Tests.Configuration.SingleTenant
{
    public abstract class Configuring_single_database<TSuite> : Scenario<TSuite> where TSuite : SingleTenantConfiguratorSuiteBase, new()
    {
        [Fact]
        public void When_database_db_connection_factory_is_not_set()
        {
            // Arrange
            var configurator = Suite.CreateSqlDatabaseConfigurator(new ServiceCollection());

            // Act
            var act = () => configurator.Apply();

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("IDbConnectionFactory type not set");
        }

        [Fact]
        public void When_database_is_configured()
        {
            // Arrange
            var services = new ServiceCollection();
            var configurator = new StreamStoreConfigurator();

            // Act
            configurator.WithSingleTenant(x => 
                    Suite.UseDatabase(x, c => 
                        c.ConfigureDatabase(x => 
                            x.WithConnectionString("connectionString")
                             .WithSchema("schema")
                             .WithTable("table"))));

            configurator.Configure(services);

            //Assert
            var provider = services.BuildServiceProvider();

            provider.GetRequiredService<IStreamDatabase>()
                     .Should().NotBeNull()
                     .And.BeOfType<SqlStreamDatabase>();

            provider.GetRequiredService<IStreamReader>().Should().NotBeNull();
            provider.GetRequiredService<ISchemaProvisioner>().Should().NotBeNull();
            provider.GetRequiredService<IDbConnectionFactory>().Should().NotBeNull();
            provider.GetRequiredService<IDapperCommandFactory>().Should().NotBeNull();
            provider.GetRequiredService<ISqlQueryProvider>().Should().NotBeNull();
            provider.GetRequiredService<ISqlProvisioningQueryProvider>().Should().NotBeNull();

            var configuration = provider.GetRequiredService<SqlDatabaseConfiguration>();
            configuration.ConnectionString.Should().Be("connectionString");
            configuration.SchemaName.Should().Be("schema");
            configuration.TableName.Should().Be("table");
        }


        [Fact]
        public void When_database_is_configured_with_appsettings()
        {
            // Arrange
            var services = new ServiceCollection();
            var configurator = new StreamStoreConfigurator();
            var appSettings = new Dictionary<string, string?>
            {
                {"ConnectionStrings:StreamStore", "connectionString"},
                {$"{Suite.SectionName}:SchemaName", "schema"},
                {$"{Suite.SectionName}:TableName", "table"}
            };

            var config = new ConfigurationBuilder()
                    .AddInMemoryCollection(appSettings)
                    .Build();
            // Act
            configurator.WithSingleTenant(x => Suite.UseDatabaseWithAppSettings(x, config));

            configurator.Configure(services);

            //Assert
            var provider = services.BuildServiceProvider();

            provider.GetRequiredService<IStreamDatabase>()
                     .Should().NotBeNull()
                     .And.BeOfType<SqlStreamDatabase>();

            provider.GetRequiredService<IStreamReader>().Should().NotBeNull();
            provider.GetRequiredService<ISchemaProvisioner>().Should().NotBeNull();
            provider.GetRequiredService<IDbConnectionFactory>().Should().NotBeNull();
            provider.GetRequiredService<IDapperCommandFactory>().Should().NotBeNull();
            provider.GetRequiredService<ISqlQueryProvider>().Should().NotBeNull();
            provider.GetRequiredService<ISqlProvisioningQueryProvider>().Should().NotBeNull();

            var configuration = provider.GetRequiredService<SqlDatabaseConfiguration>();
            configuration.ConnectionString.Should().Be("connectionString");
            configuration.SchemaName.Should().Be("schema");
            configuration.TableName.Should().Be("table");
        }
    }
}
