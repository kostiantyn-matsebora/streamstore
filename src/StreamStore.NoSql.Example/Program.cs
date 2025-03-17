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
                new KeyspaceConfiguration(Tenants.Default)
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
            new CassandraTestDatabase(new KeyspaceConfiguration(Tenants.Tenant1) { ReplicationFactor = replicationFactor }, ConfigureCluster).EnsureExists();
            new CassandraTestDatabase(new KeyspaceConfiguration(Tenants.Tenant2) { ReplicationFactor = replicationFactor }, ConfigureCluster).EnsureExists();
            new CassandraTestDatabase(new KeyspaceConfiguration(Tenants.Tenant3) { ReplicationFactor = replicationFactor }, ConfigureCluster).EnsureExists();

            // Configure the StreamStore
            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithMultitenancy(c =>
                            c.WithTenants(Tenants.Tenant1, Tenants.Tenant2, Tenants.Tenant3)
                             .UseCassandra(x =>
                                x.ConfigureDefaultCluster(ConfigureCluster)
                                 .AddKeyspace(Tenants.Tenant1, Tenants.Tenant1)
                                 .AddKeyspace(Tenants.Tenant2, Tenants.Tenant2)
                                 .AddKeyspace(Tenants.Tenant3, Tenants.Tenant3)
                                )));
        }

        static void ConfigureCosmosDbSingle(IHostApplicationBuilder appBuilder)
        {
            // Provision the keyspace
            var database = new CassandraTestDatabase(new KeyspaceConfiguration(Tenants.Default), builder => ConfigureCosmosDbBuilder(appBuilder, builder)).EnsureExists();

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
            new CassandraTestDatabase(new KeyspaceConfiguration(Tenants.Tenant1), builder => ConfigureCosmosDbBuilder(appBuilder, builder)).EnsureExists();
            new CassandraTestDatabase(new KeyspaceConfiguration(Tenants.Tenant2), builder => ConfigureCosmosDbBuilder(appBuilder, builder)).EnsureExists();
            new CassandraTestDatabase(new KeyspaceConfiguration(Tenants.Tenant3), builder => ConfigureCosmosDbBuilder(appBuilder, builder)).EnsureExists();

            // Configure the StreamStore
            appBuilder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithMultitenancy(c =>
                            c.WithTenants(Tenants.Tenant1, Tenants.Tenant2, Tenants.Tenant3)
                             .UseCassandra(x =>
                                 x.UseCosmosDb(appBuilder.Configuration, "StreamStore_CassandraCosmosDb")
                                  .AddKeyspace(Tenants.Tenant1, Tenants.Tenant1)
                                  .AddKeyspace(Tenants.Tenant2, Tenants.Tenant2)
                                  .AddKeyspace(Tenants.Tenant3, Tenants.Tenant3)
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
