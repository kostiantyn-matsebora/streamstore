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


        static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            await builder
                .ConfigureExampleApplication(c =>
                    c.EnableMultitenancy()
                     .AddDatabase(NoSqlDatabases.Cassandra,
                                  x =>
                                    x.WithSingleMode(ConfigureCassandraSingle)))
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
    }
}
