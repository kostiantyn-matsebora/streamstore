
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using StreamStore.SQL.Sqlite;
using StreamStore.Testing;

namespace StreamStore.Sql.Tests.Sqlite.Configurator
{
    public class Building_configuration : Scenario<SqliteDatabaseConfiguratorTestSuite>
    {
        public Building_configuration() : base(new SqliteDatabaseConfiguratorTestSuite())
        {
        }

        SqliteDatabaseConfigurator CreateSqliteDatabaseConfigurator()
        {
            return new SqliteDatabaseConfigurator(Suite.MockServiceCollection.Object);
        }

        [SkippableFact]
        public void When_manually_configured()
        {
            TrySkip();

            // Arrange
            var configurator = CreateSqliteDatabaseConfigurator();

            configurator
                .WithTable("tableName")
                .WithConnectionString("connectionString")
                .ProvisionSchema(true)
                .Build();

            // Act 
            var configuration = configurator.Build();
            configurator.Configure();

            // Assert
            configuration.ProvisionSchema.Should().BeTrue();
            configuration.ConnectionString.Should().Be("connectionString");
            configuration.TableName.Should().Be("tableName");

            Suite.MockRepository.VerifyAll();
        }

        [SkippableFact]
        public void When_appsettings_are_used()
        {
            TrySkip();

            // Arrange
            var configuration = Suite.MockRepository.Create<IConfiguration>();
            var section = Suite.MockRepository.Create<IConfigurationSection>();
            var connectionStrings = new Mock<IConfigurationSection>();

            var tableName = Suite.MockRepository.Create<IConfigurationSection>();
            tableName.SetupGet(x => x.Value).Returns("tableName");

            var schemaName = Suite.MockRepository.Create<IConfigurationSection>();
            schemaName.SetupGet(x => x.Value).Returns("SchemaName");

            var provisionSchema = Suite.MockRepository.Create<IConfigurationSection>();
            provisionSchema.SetupGet(x => x.Value).Returns("true");

            configuration.Setup(x => x.GetSection("ConnectionStrings")).Returns(connectionStrings.Object);
            configuration.Setup(x => x.GetSection("StreamStore:Sqlite")).Returns(section.Object);
            connectionStrings.SetupGet(x => x["StreamStore"]).Returns("connectionString");
            section.Setup(x => x.GetChildren()).Returns(new[] { section.Object });
            section.Setup(x => x.GetSection("TableName")).Returns(tableName.Object);
            section.Setup(x => x.GetSection("SchemaName")).Returns(schemaName.Object);
            section.Setup(x => x.GetSection("ProvisionSchema")).Returns(provisionSchema.Object);


            // Act
            var configurator = CreateSqliteDatabaseConfigurator();
            configurator.Configure(configuration.Object);
            var config = configurator.Build();

            // Assert
            Suite.MockRepository.VerifyAll();
            configuration.VerifyAll();
            section.VerifyAll();
            connectionStrings.VerifyAll();
            config.ConnectionString.Should().Be("connectionString");
            config.TableName.Should().Be("tableName");
            config.SchemaName.Should().Be("SchemaName");
            config.ProvisionSchema.Should().BeTrue();
        }
    }
}
