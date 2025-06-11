using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.PostgreSql;
using StreamStore.Storage;
using StreamStore.Testing;
using StreamStore.Testing.Storage.Configuration;

namespace StreamStore.Sql.Tests.PostgreSql.Configuration
{
    public class Configuring_storage : StorageConfiguratorScenario
    {
        protected override IStorageConfigurator CreateConfigurator()
        {
            return new StorageConfigurator();
        }

        [Fact]
        public void Configuring_by_extension()
        {
            // Arrange
            
            var services = new ServiceCollection();

            // Act
            services.UsePostgreSql();

            // Assert
            var provider = services.BuildServiceProvider();
            provider.GetService<IStreamStorage>().Should().NotBeNull();

            // Arrange
            services = new ServiceCollection();
            var tableName = Generated.Primitives.String;

            //Act
            services.UsePostgreSql(c => c.WithTable(tableName));

            // Assert
            provider = services.BuildServiceProvider();
            provider.GetService<IStreamStorage>().Should().NotBeNull();
            var configuration = provider.GetService<SqlStorageConfiguration>();
            configuration.Should().NotBeNull();
            configuration.TableName.Should().Be(tableName);
        }
    }
}
