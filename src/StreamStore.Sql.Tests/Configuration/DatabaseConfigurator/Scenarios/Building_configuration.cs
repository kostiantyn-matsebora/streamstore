using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using StreamStore.Sql.Configuration;
using StreamStore.Testing;

namespace StreamStore.Sql.Tests.Configuration.DatabaseConfigurator
{
    public class Building_configuration : Scenario
    {

        SqlDatabaseConfigurationBuilder CreateDatabaseConfigurationBuilder()
        {
            return new SqlDatabaseConfigurationBuilder(new SqlDatabaseConfiguration());
        }

        [SkippableFact]
        public void When_manually_configured()
        {
            TrySkip();

            // Arrange
            var builder = CreateDatabaseConfigurationBuilder();

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
            var builder = CreateDatabaseConfigurationBuilder();
            var config = builder.ReadFromConfig(configuration, "StreamStore:Sql");


            // Assert
            config.ConnectionString.Should().Be("connectionString");
            config.TableName.Should().Be("tableName");
            config.SchemaName.Should().Be("schemaName");
        }
    }
}
