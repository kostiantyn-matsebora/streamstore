using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cassandra;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Configuration.SingleTenantConfigurator.Scenarios
{
    public class Configuring_storage: Scenario
    {

        [Fact]
        public void When_configuring_storage()
        {
            // Arrange
            var configurator = new CassandraSingleTenantConfigurator();
            var services = new ServiceCollection();

            // Act
            configurator.ConfigureCluster(builder => builder.AddContactPoint("localhost").WithDefaultKeyspace("default_keyspace"));
            configurator.ConfigureStorage(c => c.WithKeyspaceName("keyspace"));
            configurator.WithSessionFactory<FakeSessionFactory>();
            configurator.Configure(services);
            var provider = services.BuildServiceProvider();
            
            // Assert
            var cluster = provider.GetRequiredService<Cluster>();
            cluster.Configuration.ClientOptions.DefaultKeyspace.Should().Be("default_keyspace");
            var sessionFactory = provider.GetRequiredService<ICassandraSessionFactory>();
            sessionFactory.Should().BeOfType<FakeSessionFactory>();
            var config = provider.GetRequiredService<CassandraStorageConfiguration>();
            config.Keyspace.Should().Be("keyspace");
            provider.GetRequiredService<ICassandraStreamRepositoryFactory>().Should().NotBeNull();
            
        }

        [Fact]
        public void When_cluster_is_not_configured()
        {
            // Arrange
            var configurator = new CassandraSingleTenantConfigurator();
            var services = new ServiceCollection();

            // Act
            var act = () => configurator.Configure(services);


            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        class FakeSessionFactory : ICassandraSessionFactory
        {
            public ISession Open()
            {
                throw new NotImplementedException();
            }
        }
    }
}
