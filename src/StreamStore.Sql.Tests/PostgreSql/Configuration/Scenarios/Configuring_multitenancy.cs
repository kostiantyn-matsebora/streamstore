using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.PostgreSql;
using StreamStore.Storage;
using StreamStore.Testing.Storage.Configuration;
using StreamStore.Testing;
using StreamStore.Multitenancy;
using StreamStore.Sql.Multitenancy;

namespace StreamStore.Sql.Tests.PostgreSql.Configuration
{
    public class Configuring_multitenancy: MultitenancyConfiguratorScenario
    {
        protected override IMultitenancyConfigurator CreateConfigurator()
        {
            return new MultitenancyConfigurator((c) => { });
        }

        protected override void ConfigureRequiredDependencies(IServiceCollection services)
        {
            services.AddSingleton(new SqlStorageConfiguration());
            services.AddSingleton(new MigrationConfiguration());
        }

        [Fact]
        public void Configuring_by_extension()
        {
            // Arrange
            var tenantId = Generated.Primitives.Id;
            var services = new ServiceCollection();
            var connectionString = Generated.Primitives.String;

            // Act
            services.UsePostgreSqlWithMultitenancy(c => c.WithConnectionString(tenantId, connectionString));

            // Assert
            var provider = services.BuildServiceProvider();
            var storageProvider = provider.GetService<ITenantStreamStorageProvider>();
                
            storageProvider.Should().NotBeNull();

            var connectionStringProvider = provider.GetService<ISqlTenantConnectionStringProvider>();
            connectionStringProvider.Should().NotBeNull();

            var result = connectionStringProvider.GetConnectionString(tenantId);
            result.Should().NotBeNullOrEmpty()
                .And.Be(connectionString);

            // Arrange
            services = new ServiceCollection();
            var config = new SqlStorageConfiguration
            {
                ConnectionString = connectionString
            };

            // Act
            services.UsePostgreSqlWithMultitenancy(config, c => { });

            // Assert
            provider = services.BuildServiceProvider();
            provider.GetService<ITenantStreamStorageProvider>().Should().NotBeNull();
            var resultConfig = provider.GetService<SqlStorageConfiguration>();
            resultConfig.Should().NotBeNull();
            resultConfig.ConnectionString.Should().Be(connectionString);
        }
    }
}
