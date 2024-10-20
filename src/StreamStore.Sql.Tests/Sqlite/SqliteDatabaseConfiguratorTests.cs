using System.Xml.Linq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StreamStore.SQL.Sqlite;

namespace StreamStore.Sql.Tests.Sqlite
{
    public class SqliteDatabaseConfiguratorTests
    {
        readonly MockRepository mockRepository;

        readonly Mock<IServiceCollection> mockServiceCollection;

        public SqliteDatabaseConfiguratorTests()
        {
            mockRepository = new MockRepository(MockBehavior.Default);

            mockServiceCollection = mockRepository.Create<IServiceCollection>();
        }

        SqliteDatabaseConfigurator CreateSqliteDatabaseConfigurator()
        {
            return new SqliteDatabaseConfigurator(mockServiceCollection.Object);
        }

        [Fact]
        public void Configure_Should_CreateFullConfiguration()
        {
            // Arrange
            var configurator = CreateSqliteDatabaseConfigurator();
            configurator
                .WithTable("tableName")
                .WithConnectionString("connectionString")
                .ProvisionSchema(true)
                .EnableProfiling()
                .Build();

            // Act 
            var configuration = configurator.Build();
            configurator.Configure();

            // Assert
            configuration.EnableProfiling.Should().BeTrue();
            configuration.ProvisionSchema.Should().BeTrue();
            configuration.ConnectionString.Should().Be("connectionString");
            configuration.TableName.Should().Be("tableName");

            mockRepository.VerifyAll();
        }

        [Fact]
        public void Configure_Should_UseAppsettings()
        {
            // Arrange
            var configuration = mockRepository.Create<IConfiguration>();
            var section = mockRepository.Create<IConfigurationSection>();
            var connectionStrings = new Mock<IConfigurationSection>();

            var tableName = mockRepository.Create<IConfigurationSection>();
            tableName.SetupGet(x => x.Value).Returns("tableName");

            var schemaName = mockRepository.Create<IConfigurationSection>();
            schemaName.SetupGet(x => x.Value).Returns("SchemaName");

            var provisionSchema = mockRepository.Create<IConfigurationSection>();
            provisionSchema.SetupGet(x => x.Value).Returns("true");

            var profilingEnabled = mockRepository.Create<IConfigurationSection>();
            profilingEnabled.SetupGet(x => x.Value).Returns("true");

            configuration.Setup(x => x.GetSection("ConnectionStrings")).Returns(connectionStrings.Object);
            configuration.Setup(x => x.GetSection("StreamStore:Sqlite")).Returns(section.Object);
            connectionStrings.SetupGet(x => x["StreamStore"]).Returns("connectionString");
            section.Setup(x => x.GetChildren()).Returns(new[] { section.Object });
            section.Setup(x => x.GetSection("TableName")).Returns(tableName.Object);
            section.Setup(x => x.GetSection("SchemaName")).Returns(schemaName.Object);
            section.Setup(x => x.GetSection("ProvisionSchema")).Returns(provisionSchema.Object);
            section.Setup(x => x.GetSection("ProfilingEnabled")).Returns(profilingEnabled.Object);


            // Act
            var configurator = CreateSqliteDatabaseConfigurator();
            configurator.Configure(configuration.Object);
            var config = configurator.Build();

            // Assert
            mockRepository.VerifyAll();
            configuration.VerifyAll();
            section.VerifyAll();
            connectionStrings.VerifyAll();
            config.ConnectionString.Should().Be("connectionString");
            config.TableName.Should().Be("tableName");
            config.SchemaName.Should().Be("SchemaName");
            config.ProvisionSchema.Should().BeTrue();
            config.EnableProfiling.Should().BeTrue();
        }
    }

}
