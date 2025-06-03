using FluentAssertions;
using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;
using StreamStore.Testing;

namespace StreamStore.Sql.Tests.Configuration.StorageConfigurator
{
    public class Building_configuration : Scenario
    {
        static SqlStorageConfigurationBuilder CreateStorageConfigurationBuilder()
        {
            return new SqlStorageConfigurationBuilder(new SqlStorageConfiguration());
        }

        [SkippableFact]
        public void When_manually_configured()
        {
            TrySkip();

            // Arrange
            var builder = CreateStorageConfigurationBuilder();

            builder
                .WithConnectionString("connectionString")
                .WithTable("tableName")
                .WithSchema("schemaName");

            // Act 
            var configuration = builder.Build();

            // Assert
            configuration.ConnectionString.Should().Be("connectionString");
            configuration.TableName.Should().Be("tableName");
            configuration.SchemaName.Should().Be("schemaName");
        }

        [SkippableFact]
        public void When_appsettings_are_used()
        {
            TrySkip();

            // Arrange
            var inMemoryConfig = new Dictionary<string, string?>()
                {
                    { "ConnectionStrings:StreamStore", "connectionString" },
                    { "StreamStore:Sql:TableName", "tableName" },
                    { "StreamStore:Sql:SchemaName", "schemaName" }
                };

            var configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemoryConfig).Build();

            // Act
            var config = SqlStorageConfigurationBuilder.ReadFromConfig(configuration, "StreamStore:Sql");


            // Assert
            config.ConnectionString.Should().Be("connectionString");
            config.TableName.Should().Be("tableName");
            config.SchemaName.Should().Be("schemaName");
        }
    }
}
