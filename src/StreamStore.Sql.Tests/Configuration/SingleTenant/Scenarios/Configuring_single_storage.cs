using FluentAssertions;
using StreamStore.Testing;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Storage;
using StreamStore.Provisioning;
using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;
using Microsoft.Extensions.Configuration;
using StreamStore.Configuration;



namespace StreamStore.Sql.Tests.Configuration.SingleTenant
{
    public abstract class Configuring_single_storage<TEnvironment> : Scenario<TEnvironment> where TEnvironment : SingleTenantConfiguratorTestEnvironmentBase, new()
    {
        [Fact]
        public void When_storage_db_connection_factory_is_not_set()
        {
            // Arrange
            var configurator = Environment.CreateSqlStorageConfigurator(new ServiceCollection());

            // Act
            var act = () => configurator.Apply();

            //Assert
            act.Should().Throw<InvalidOperationException>().WithMessage("IDbConnectionFactory type not set");
        }

        [Fact]
        public void When_storage_is_configured()
        {
            // Arrange
            var services = new ServiceCollection();
            var configurator = ConfiguratorFactory.StoreConfigurator;

            // Act
            configurator.WithSingleStorage(x => 
                    Environment.UseParticularStorage(x, c => 
                        c.ConfigureStorage(x => 
                            x.WithConnectionString("connectionString")
                             .WithSchema("schema")
                             .WithTable("table"))));

            configurator.Configure(services);

            //Assert
            var provider = services.BuildServiceProvider();

            provider.GetRequiredService<IStreamStorage>()
                     .Should().NotBeNull()
                     .And.BeOfType<SqlStreamStorage>();

            provider.GetRequiredService<IStreamReader>().Should().NotBeNull();
            provider.GetRequiredService<ISchemaProvisioner>().Should().NotBeNull();
            provider.GetRequiredService<IDbConnectionFactory>().Should().NotBeNull();
            provider.GetRequiredService<IDapperCommandFactory>().Should().NotBeNull();
            provider.GetRequiredService<ISqlQueryProvider>().Should().NotBeNull();
            provider.GetRequiredService<ISqlProvisioningQueryProvider>().Should().NotBeNull();

            var configuration = provider.GetRequiredService<SqlStorageConfiguration>();
            configuration.ConnectionString.Should().Be("connectionString");
            configuration.SchemaName.Should().Be("schema");
            configuration.TableName.Should().Be("table");
        }


        [Fact]
        public void When_storage_is_configured_with_appsettings()
        {
            // Arrange
            var services = new ServiceCollection();
            var configurator = ConfiguratorFactory.StoreConfigurator;
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
            configurator.WithSingleStorage(x => Environment.UseParticularStorageWithAppSettings(x, config));

            configurator.Configure(services);

            //Assert
            var provider = services.BuildServiceProvider();

            provider.GetRequiredService<IStreamStorage>()
                     .Should().NotBeNull()
                     .And.BeOfType<SqlStreamStorage>();

            provider.GetRequiredService<IStreamReader>().Should().NotBeNull();
            provider.GetRequiredService<ISchemaProvisioner>().Should().NotBeNull();
            provider.GetRequiredService<IDbConnectionFactory>().Should().NotBeNull();
            provider.GetRequiredService<IDapperCommandFactory>().Should().NotBeNull();
            provider.GetRequiredService<ISqlQueryProvider>().Should().NotBeNull();
            provider.GetRequiredService<ISqlProvisioningQueryProvider>().Should().NotBeNull();

            var configuration = provider.GetRequiredService<SqlStorageConfiguration>();
            configuration.ConnectionString.Should().Be("connectionString");
            configuration.SchemaName.Should().Be("schema");
            configuration.TableName.Should().Be("table");
        }
    }
}
