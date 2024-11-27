using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using StreamStore.ExampleBase;

using System.CommandLine;
using StreamStore.NoSql.Example;
using StreamStore.NoSql.Tests.Cassandra.Database;
using StreamStore.NoSql.Cassandra;
using System.Security.Authentication;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Cassandra;

namespace StreamStore.Sql.Example
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        const string singleDatabaseName = "streamstore";
        readonly static Id tenant1 = "tenant_1";
        readonly static Id tenant2 = "tenant_2";
        readonly static Id tenant3 = "tenant_3";

        // Define your CosmosDB settings here for CosmosDB Cassandra API cases
        const string CosmosDbAccount = "streamstore";
        const string CosmosDbUsername = "";
        const string CosmosDbPassword = "";
        const string CosmosDbContractPoint = $"{CosmosDbAccount}.cassandra.cosmos.azure.com";
        const int CosmosDbPort = 10350;

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
            var database = new CassandraTestDatabase(singleDatabaseName);
            database.EnsureExists();

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithSingleDatabase(c =>
                        c.UseCassandra(x =>
                            x.ConfigureCluster(c =>
                                c.AddContactPoint("localhost")
                                 .WithQueryTimeout(10_000)
                            )
                        )
                    )
                 );
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
                                x.ConfigureDefaultCluster(c =>
                                    c.AddContactPoint("localhost")
                                     .WithQueryTimeout(10_000)
                                  )
                                 .AddKeyspace(tenant1, tenant1)
                                 .AddKeyspace(tenant2, tenant2)
                                 .AddKeyspace(tenant3, tenant3)
                                )));

        }

        static void ConfigureCosmosDbSingle(IHostApplicationBuilder builder)
        {
            var database = new CassandraTestDatabase(singleDatabaseName, ConfigureCosmosDbBuilder);

            database.EnsureExists();

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                        .WithSingleDatabase(c =>
                        c.UseCosmosDbCassandra(x =>
                           x.WithCosmosDbAccount(CosmosDbAccount)
                            .WithCredentials(CosmosDbUsername, CosmosDbPassword)
                        )
                    )
                );
        }

        static void ConfigureCosmosDbMultitenancy(IHostApplicationBuilder builder)
        {
            new CassandraTestDatabase(tenant1, ConfigureCosmosDbBuilder).EnsureExists();
            new CassandraTestDatabase(tenant2, ConfigureCosmosDbBuilder).EnsureExists();
            new CassandraTestDatabase(tenant3, ConfigureCosmosDbBuilder).EnsureExists();

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithMultitenancy(c =>
                            c.WithTenants(tenant1, tenant2, tenant3)
                             .UseCosmosDbCassandra(x =>
                                 x.WithCosmosDbAccount(CosmosDbAccount)
                                  .WithCredentials(CosmosDbUsername, CosmosDbPassword)
                                  .AddKeyspace(tenant1, tenant1)
                                  .AddKeyspace(tenant2, tenant2)
                                  .AddKeyspace(tenant3, tenant3)
                                )));

        }

        static void ConfigureCosmosDbBuilder(Builder builder)
        {
            var options = new SSLOptions(SslProtocols.Tls12, true, remoteCertValidationCallback: ValidateServerCertificate);

            options.SetHostNameResolver((ipAddress) => CosmosDbContractPoint);

            builder
            .WithCredentials(CosmosDbUsername, CosmosDbPassword)
                .WithPort(CosmosDbPort)
                .AddContactPoint(CosmosDbContractPoint)
                .WithSSL(options);
        }
        public static bool ValidateServerCertificate(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None) return true;
            return false;
        }
    }
}
