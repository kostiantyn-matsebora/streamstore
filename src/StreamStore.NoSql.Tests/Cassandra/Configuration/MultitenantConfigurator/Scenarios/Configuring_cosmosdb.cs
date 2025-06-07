
//using Cassandra;
//using FluentAssertions;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using StreamStore.NoSql.Cassandra.Configuration;
//using StreamStore.NoSql.Cassandra.Multitenancy;
//using StreamStore.Testing;

//namespace StreamStore.NoSql.Tests.Cassandra.Configuration.MultitenantConfigurator;

//public class Configuring_cosmosdb: Scenario
//{

//    [Fact]
//    public void When_connection_string_is_not_set()
//    {
//        // Arrange
//        var configuration = new ConfigurationBuilder().Build();
//        var configurator = new CassandraMultitenantConfigurator();

//        // Act
//        var act = () => configurator.UseCosmosDb(configuration);

//        //Assert
//        act.Should().Throw<ArgumentException>();
//    }


//    [Fact]
//    public void When_connection_string_is_defined()
//    {
//        // Arrange
//        var configurator = new CassandraMultitenantConfigurator();
//        var services = new ServiceCollection();
//        var connectionString = "HostName=azure.com;Username=streamstore;Password=xxxxxxxxxxx;Port=10350";

//        var inMemoryConfig = new Dictionary<string, string?>()
//            {
//                { "ConnectionStrings:StreamStore", connectionString },
//            };
//        var configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemoryConfig).Build();

//        // Act
//        configurator
//            .UseCosmosDb(configuration)
//            .WithKeyspaceProvider<FakeKeyspaceProvider>();
//        configurator.Configure(services);
//        var provider = services.BuildServiceProvider();

//        // Assert
//        var clusterConfigurator = provider.GetRequiredService<IClusterConfigurator>();
//        var builder = Cluster.Builder();
//        clusterConfigurator.Configure(builder);
//        var cluster = builder.Build();
//        cluster.Should().NotBeNull();

//        // Arrange
//        configurator = new CassandraMultitenantConfigurator();
//        services = new ServiceCollection();

//        // Act
//        configurator
//            .UseCosmosDb(connectionString)
//            .WithKeyspaceProvider<FakeKeyspaceProvider>();
//        configurator.Configure(services);
//        provider = services.BuildServiceProvider();

//        // Assert
//        clusterConfigurator = provider.GetRequiredService<IClusterConfigurator>();
//        builder = Cluster.Builder();
//        clusterConfigurator.Configure(builder);
//        cluster = builder.Build();
//        cluster.Should().NotBeNull();
//    }

//}
