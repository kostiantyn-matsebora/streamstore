using Cassandra.Mapping;
using Moq;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Provisioning;
using StreamStore.Testing.Framework;

namespace StreamStore.NoSql.Tests.Cassandra.Provisioning;

public class SchemaProvisionerSuite: TestSuite
{

    internal Mock<ICassandraMapperProvider> MapperProvider { get; }
    internal Mock<IMapper> Mapper { get; }
    internal CassandraSchemaProvisioner SchemaProvisioner => new CassandraSchemaProvisioner(MapperProvider.Object, new CassandraStorageConfiguration());

    public SchemaProvisionerSuite()
    {
        MapperProvider = MockRepository.Create<ICassandraMapperProvider>();
        Mapper = MockRepository.Create<IMapper>();
        MapperProvider.Setup(x => x.OpenMapper()).Returns(Mapper.Object);
    }
}
