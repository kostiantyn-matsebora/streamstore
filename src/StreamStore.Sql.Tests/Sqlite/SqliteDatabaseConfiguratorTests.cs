using Dapper.Extensions.Monitor;
using Dapper.Extensions.SQLite;
using FluentAssertions;
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
    }
}
