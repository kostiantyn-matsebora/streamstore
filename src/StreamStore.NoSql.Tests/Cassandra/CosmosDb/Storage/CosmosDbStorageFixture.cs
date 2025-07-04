using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Cassandra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra;
using StreamStore.NoSql.Cassandra.Extensions;
using StreamStore.NoSql.Tests.Cassandra.Storage;
using StreamStore.Testing;
using StreamStore.Testing.Framework;

namespace StreamStore.NoSql.Tests.Cassandra.CosmosDb.Storage
{
    public class CosmosDbStorageFixture : StorageFixtureBase<CassandraTestStorage>
    {
        const string connectionStringName = "StreamStore_CassandraCosmosDb";
        public CosmosDbStorageFixture() : this(new CassandraTestStorage(KeyspaceConfiguration(), ConfigureCluster))
        {
        }

        CosmosDbStorageFixture(CassandraTestStorage storage) : base(storage)
        {

        }

        static void ConfigureCluster(Builder builder)
        {
            builder.WithCosmosDbConnectionString(GetConfiguration().GetConnectionString(connectionStringName)!, ValidateServerCertificate!);
        }

        static KeyspaceConfiguration KeyspaceConfiguration()
        {
            // Put your keyspace configuration here
            return new KeyspaceConfiguration(Generated.Names.Storage)
            {
                ReplicationClass = "SimpleStrategy",
                ReplicationFactor = 1
            };
        }

        public override void ConfigurePersistence(IServiceCollection services)
        {
            services.UseCassandra(c =>
                         c.UseCosmosDb(GetConfiguration(), connectionStringName, ValidateServerCertificate!)
                          .ConfigureStorage(k => k.WithKeyspaceName(testStorage.Keyspace.Name)));
        }

       static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                     .AddJsonFile("appsettings.json", true)
                     .AddJsonFile("appsettings.Development.json", true)
                     .Build();
        }

        protected override InMemoryStreamContainer CreateInMemoryContainer()
        {
            return new InMemoryStreamContainer(new InMemoryStreamContainerOptions { Capacity = 100, EventPerStream = 100 });
        }

        static bool ValidateServerCertificate(
#pragma warning disable S1172 // Unused method parameters should be removed
           object sender,
           X509Certificate certificate,
           X509Chain chain,
           SslPolicyErrors sslPolicyErrors)
#pragma warning restore S1172 // Unused method parameters should be removed
        {
            return true;
        }


    }
}
