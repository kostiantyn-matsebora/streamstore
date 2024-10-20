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
            mockRepository = new MockRepository(MockBehavior.Strict);

            mockServiceCollection = mockRepository.Create<IServiceCollection>();
        }

        SqliteDatabaseConfigurator CreateSqliteDatabaseConfigurator()
        {
            return new SqliteDatabaseConfigurator(
                mockServiceCollection.Object);
        }

        [Fact]
        public void Configure_Should_CreateFullConfiguration()
        {
            // Arrange and Act
            var configuration = CreateSqliteDatabaseConfigurator()
                .WithTable("tableName")
                .WithConnectionString("connectionString")
                .WithSchemaProvisioning(true)
                .WithProfiling()
                .Build();

            

            // Assert
            configuration.EnableProfiling.Should().BeTrue();
            configuration.ProvisionSchema.Should().BeTrue();
            configuration.ConnectionString.Should().Be("connectionString");
            configuration.TableName.Should().Be("tableName");

            mockRepository.VerifyAll();
        }
    }
}
