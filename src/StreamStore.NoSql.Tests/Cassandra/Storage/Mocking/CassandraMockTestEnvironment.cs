using Cassandra.Mapping;
using Moq;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Storage;
using StreamStore.Testing;
using StreamStore.Testing.Framework;

namespace StreamStore.NoSql.Tests.Cassandra.Storage.Mocking
{
    public class CassandraMockTestEnvironment : TestEnvironment
    {

        internal readonly Mock<ICassandraMapperProvider> MapperProvider;
        internal readonly Mock<ICassandraCqlQueries> Queries;
        internal readonly Mock<IMapper> Mapper;
        internal readonly CassandraStreamStorage StreamStorage;
        internal CassandraStreamWriter StreamUnitOfWork =>
            new CassandraStreamWriter(Generated.Primitives.Id, Generated.Primitives.Revision, null, Mapper.Object, 
                    new CassandraStatementConfigurator(new CassandraStorageConfiguration()),
                    new CassandraCqlQueries(new CassandraStorageConfiguration()));

        public CassandraMockTestEnvironment()
        {
            MapperProvider = MockRepository.Create<ICassandraMapperProvider>();
            Mapper = MockRepository.Create<IMapper>();
            Queries = MockRepository.Create<ICassandraCqlQueries>();
            MapperProvider.Setup(x => x.OpenMapper()).Returns(Mapper.Object);
            StreamStorage = new CassandraStreamStorage(MapperProvider.Object, Queries.Object, new CassandraStorageConfiguration());
        }
    }
}
