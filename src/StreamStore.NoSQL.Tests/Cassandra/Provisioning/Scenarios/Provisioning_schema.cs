using FluentAssertions;
using Moq;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Provisioning;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Provisioning;

public class Provisioning_schema: Scenario<SchemaProvisionerSuite>
{
    [Fact]
    public void When_any_parameter_is_not_set() {

        // Act
        var act = () => new CassandraSchemaProvisioner(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    
    }

    [Fact]
    public async Task When_provisioning() {
        // Arange
        var repoFactory = Suite.RepositoryFactory;
        var repo = Suite.MockRepository.Create<ICassandraStreamRepository>();
        repo.Setup(x => x.CreateSchemaIfNotExistsAsync()).Returns(Task.CompletedTask);
        repoFactory.Setup(x => x.Create()).Returns(repo.Object);
        var provisioner = new CassandraSchemaProvisioner(repoFactory.Object);
        
        // Act
        await provisioner.ProvisionSchemaAsync(CancellationToken.None);

        // Assert
        Suite.MockRepository.VerifyAll();
    }
} 
