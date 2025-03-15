using System.Diagnostics.CodeAnalysis;
using System.CommandLine;
using Microsoft.Extensions.Hosting;

using StreamStore.ExampleBase;
using StreamStore.NoSql.Example;
using StreamStore.NoSql.Tests.Cassandra.Database;
using StreamStore.NoSql.Cassandra;
using StreamStore.NoSql.Cassandra.Extensions;

using Cassandra;
using Microsoft.Extensions.Configuration;

namespace StreamStore.Sql.Example
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        const string singleDatabaseName = "streamstore";
        readonly static Id tenant1 = "tenant_1";
        readonly static Id tenant2 = "tenant_2";
        readonly static Id tenant3 = "tenant_3";
        readonly static int replicationFactor = 3;
        static async Task Main(string[] args)
        {
            var builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder(args);
            await builder
                .ConfigureExampleApplication(c =>
                    c.EnableMultitenancy()
                     .AddDatabase(NoSqlDatabases.Cassandra,
                                  x => x.WithSingleMode(ConfigureCassandraSingle)
                                        .WithMultitenancy(ConfigureCassandraMultitenancy))
                     .AddDatabase(NoSqlDatabases.CosmosDbCassandra,
                                  x => x.WithSingleMode(ConfigureCosmosDbSingle)
                                        .WithMultitenancy(ConfigureCosmosDbMultitenancy)))
                .InvokeAsync(args);
        }

        static void ConfigureCassandraSingle(IHostApplicationBuilder builder)
        {
            // Provision the keyspace
            var database = new CassandraTestDatabase(
                new KeyspaceConfiguration(singleDatabaseName)
                {
                    ReplicationFactor = replicationFactor
                }, ConfigureCluster);

            database.EnsureExists();

            // Configure the StreamStore
            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithSingleDatabase(c =>
                        c.UseCassandra(x =>
                            x.ConfigureCluster(ConfigureCluster)
                        )
                    )
                 );
        }

        static void ConfigureCluster(Builder builder)
        {
            // Add your custom cluster configuration here
            builder.AddContactPoint("localhost");
        }

        static void ConfigureCassandraMultitenancy(IHostApplicationBuilder builder)
        {
            // Provision the tenant keyspaces
            new CassandraTestDatabase(new KeyspaceConfiguration(tenant1) { ReplicationFactor = replicationFactor }, ConfigureCluster).EnsureExists();
            new CassandraTestDatabase(new KeyspaceConfiguration(tenant2) { ReplicationFactor = replicationFactor }, ConfigureCluster).EnsureExists();
            new CassandraTestDatabase(new KeyspaceConfiguration(tenant3) { ReplicationFactor = replicationFactor }, ConfigureCluster).EnsureExists();

            // Configure the StreamStore
            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithMultitenancy(c =>
                            c.WithTenants(tenant1, tenant2, tenant3)
                             .UseCassandra(x =>
                                x.ConfigureDefaultCluster(ConfigureCluster)
                                 .AddKeyspace(tenant1, tenant1)
                                 .AddKeyspace(tenant2, tenant2)
                                 .AddKeyspace(tenant3, tenant3)
                                )));
        }

        static void ConfigureCosmosDbSingle(IHostApplicationBuilder appBuilder)
        {
            // Provision the keyspace
            var database = new CassandraTestDatabase(new KeyspaceConfiguration(singleDatabaseName), builder => ConfigureCosmosDbBuilder(appBuilder, builder)).EnsureExists();

            // Configure the StreamStore
            appBuilder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                        .WithSingleDatabase(c =>
                            c.UseCassandra(x =>
                                x.UseCosmosDb(appBuilder.Configuration, "StreamStore_CassandraCosmosDb")
                        )
                    )
                );
        }

        static void ConfigureCosmosDbMultitenancy(IHostApplicationBuilder appBuilder)
        {
            // Provision the tenant keyspaces
            new CassandraTestDatabase(new KeyspaceConfiguration(tenant1), builder => ConfigureCosmosDbBuilder(appBuilder, builder)).EnsureExists();
            new CassandraTestDatabase(new KeyspaceConfiguration(tenant2), builder => ConfigureCosmosDbBuilder(appBuilder, builder)).EnsureExists();
            new CassandraTestDatabase(new KeyspaceConfiguration(tenant3), builder => ConfigureCosmosDbBuilder(appBuilder, builder)).EnsureExists();

            // Configure the StreamStore
            appBuilder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithMultitenancy(c =>
                            c.WithTenants(tenant1, tenant2, tenant3)
                             .UseCassandra(x =>
                                 x.UseCosmosDb(appBuilder.Configuration, "StreamStore_CassandraCosmosDb")
                                  .AddKeyspace(tenant1, tenant1)
                                  .AddKeyspace(tenant2, tenant2)
                                  .AddKeyspace(tenant3, tenant3)
                                )));
        }

        static void ConfigureCosmosDbBuilder(IHostApplicationBuilder appBuilder, Builder builder)
        {
            var connectionString = appBuilder.Configuration.GetConnectionString("StreamStore_CassandraCosmosDb");
            if (connectionString == null) throw new InvalidOperationException("Connection string StreamStore_CassandraCosmosDb is not found");

            builder.WithCosmosDbConnectionString(connectionString);
        }
    }
}
