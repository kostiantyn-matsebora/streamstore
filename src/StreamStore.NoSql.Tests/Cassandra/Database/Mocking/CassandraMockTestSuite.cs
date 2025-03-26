using Cassandra.Mapping;
using Moq;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Testing;
using StreamStore.Testing.Framework;

namespace StreamStore.NoSql.Tests.Cassandra.Database.Mocking
{
    public class CassandraMockTestSuite : TestSuite
    {

        internal readonly Mock<ICassandraMapperProvider> MapperProvider;
        internal readonly Mock<ICassandraCqlQueries> Queries;
        internal readonly Mock<IMapper> Mapper;
        internal readonly CassandraStreamDatabase StreamDatabase;
        internal CassandraStreamUnitOfWork StreamUnitOfWork =>
            new CassandraStreamUnitOfWork(Generated.Primitives.Id, Generated.Primitives.Revision, null, Mapper.Object, 
                    new CassandraStatementConfigurator(new CassandraStorageConfiguration()),
                    new CassandraCqlQueries(new CassandraStorageConfiguration()));

        public CassandraMockTestSuite()
        {
            MapperProvider = MockRepository.Create<ICassandraMapperProvider>();
            Mapper = MockRepository.Create<IMapper>();
            Queries = MockRepository.Create<ICassandraCqlQueries>();
            //Mapper.Setup(x => x.Dispose());
            MapperProvider.Setup(x => x.OpenMapper()).Returns(Mapper.Object);
            StreamDatabase = new CassandraStreamDatabase(MapperProvider.Object, Queries.Object, new CassandraStorageConfiguration());
        }
    }
}
