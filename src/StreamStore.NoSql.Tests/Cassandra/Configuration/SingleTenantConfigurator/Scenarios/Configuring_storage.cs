//using Cassandra;
//using FluentAssertions;
//using Microsoft.Extensions.DependencyInjection;
//using StreamStore.NoSql.Cassandra.API;
//using StreamStore.NoSql.Cassandra.Configuration;
//using StreamStore.NoSql.Cassandra.Storage;
//using StreamStore.Testing;

//namespace StreamStore.NoSql.Tests.Cassandra.Configuration.SingleTenantConfigurator
//{
//    public class Configuring_storage : Scenario
//    {

//        [Fact]
//        public void When_configuring_storage()
//        {
//            // Arrange
//            var configurator = new CassandraSingleTenantConfigurator();
//            var services = new ServiceCollection();

//            // Act
//            configurator.ConfigureCluster(builder => builder.AddContactPoint("localhost").WithDefaultKeyspace("default_keyspace"));
//            configurator.ConfigurePersistence(c => c.WithKeyspaceName("keyspace"));
//            configurator.WithSessionFactory<FakeSessionFactory>();
//            configurator.Configure(services);
//            var provider = services.BuildServiceProvider();

//            // Assert
//            provider.GetRequiredService<ICluster>().Configuration.ClientOptions.DefaultKeyspace
//                    .Should().Be("default_keyspace");
//            provider.GetRequiredService<ICassandraSessionFactory>()
//                    .Should().BeOfType<FakeSessionFactory>();
//            provider.GetRequiredService<ICassandraMapperProvider>().Should().NotBeNull();
//            provider.GetRequiredService<CassandraStorageConfiguration>().Keyspace
//                    .Should().Be("keyspace");
//        }

//        class FakeSessionFactory : ICassandraSessionFactory
//        {
//            public ISession Open()
//            {
//                throw new NotImplementedException();
//            }
//        }
//    }
//}
