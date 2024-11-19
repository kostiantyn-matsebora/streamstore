using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using StreamStore.ExampleBase;

using System.CommandLine;
using StreamStore.NoSql.Example;
using StreamStore.NoSql.Tests.Cassandra.Database;
using StreamStore.NoSql.Cassandra;

namespace StreamStore.Sql.Example
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        const string singleDatabaseName = "streamstore";
        readonly static Id tenant1 = "tenant_1";
        readonly static Id tenant2 = "tenant_2";
        readonly static Id tenant3 = "tenant_3";

        static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            await builder
                .ConfigureExampleApplication(c =>
                    c.EnableMultitenancy()
                     .AddDatabase(NoSqlDatabases.Cassandra,
                                  x => x.WithSingleMode(ConfigureCassandraSingle)
                                        .WithMultitenancy(ConfigureCassandraMultitenancy)))
                .InvokeAsync(args);
        }

        static void ConfigureCassandraSingle(IHostApplicationBuilder builder)
        {
            var database = new CassandraTestDatabase(singleDatabaseName);
            database.EnsureExists();

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithSingleDatabse(c =>
                        c.UseCassandra(x => 
                            x.ConfigureCluster(c => 
                                c.AddContactPoint("localhost")))));
        }

        static void ConfigureCassandraMultitenancy(IHostApplicationBuilder builder)
        {
            new CassandraTestDatabase(tenant1).EnsureExists();
            new CassandraTestDatabase(tenant2).EnsureExists();
            new CassandraTestDatabase(tenant3).EnsureExists();


            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithMultitenancy(c =>
                            c.WithTenants(tenant1, tenant2, tenant3)
                             .UseCassandra(x =>
                                x.ConfigureDefaultCluster(c => c.AddContactPoint("localhost"))
                                 .AddKeyspace(tenant1, tenant1)
                                 .AddKeyspace(tenant2, tenant2)
                                 .AddKeyspace(tenant3, tenant3)
                                )));

        }
    }
}
