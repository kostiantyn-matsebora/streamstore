//using Cassandra;
//using FluentAssertions;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using StreamStore.NoSql.Cassandra.Configuration;
//using StreamStore.Testing;

//namespace StreamStore.NoSql.Tests.Cassandra.Configuration.SingleTenantConfigurator
//{
//    public class Configuring_cosmosdb: Scenario
//    {
//        [Fact]
//        public void When_connection_string_is_not_set()
//        {
//            // Arrange
//            var configuration = new ConfigurationBuilder().Build();
//            var configurator = new CassandraSingleTenantConfigurator();

//            // Act
//            var act = () => configurator.UseCosmosDb(configuration);

//            //Assert
//            act.Should().Throw<ArgumentException>();

//            //Act
//            act = () => configurator.UseCosmosDb((string)null!);

//            //Assert
//            act.Should().Throw<ArgumentException>();


//        }


//        [Fact]
//        public void When_connection_string_is_defined()
//        {
//            // Arrange
//            var connectionString = "HostName=azure.com;Username=streamstore;Password=xxxxxxxxxxx;Port=10350";

//            var inMemoryConfig = new Dictionary<string, string?>()
//                {
//                    { "ConnectionStrings:StreamStore", connectionString },
//                };
//            var configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemoryConfig).Build();
//            var configurator = new CassandraSingleTenantConfigurator();
//            var services = new ServiceCollection();

//            // Act
//            configurator.UseCosmosDb(configuration).Configure(services);
//            var serviceProvider = services.BuildServiceProvider();

//            //Assert
//            var cluster = serviceProvider.GetRequiredService<ICluster>();
//            cluster.Should().NotBeNull();

//            // Arrange
//            configurator = new CassandraSingleTenantConfigurator();
//            services = new ServiceCollection();

//            // Act
//            configurator.UseCosmosDb(connectionString).Configure(services);
//            serviceProvider = services.BuildServiceProvider();

//            //Assert
//            cluster = serviceProvider.GetRequiredService<ICluster>();
//            cluster.Should().NotBeNull();
//        }
//    }
//}
