using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using StreamStore.ExampleBase;
using StreamStore.Sql.Sqlite;
using StreamStore.Sql.PostgreSql;
using StreamStore.Sql.Tests.PostgreSql.Storage;
using StreamStore.Sql.Tests.Sqlite.Storage;
using System.CommandLine;
using StreamStore.Sql.Tests.Storage;

namespace StreamStore.Sql.Example
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            await builder
                .ConfigureExampleApplication(c =>
                    c.EnableMultitenancy()
                     .AddStorage(SqlStorages.SQLite,
                                  x => 
                                    x.WithSingleMode(ConfigureSqliteSingle)
                                     .WithMultitenancy(ConfigureSqliteMultitenancy))
                     .AddStorage(SqlStorages.PostgreSQL,
                                   x => 
                                       x.WithSingleMode(ConfigurePostgresSingle)
                                        .WithMultitenancy(ConfigurePostgresMultitenancy)))
                .InvokeAsync(args);
        }

        static void ConfigureSqliteSingle(IHostApplicationBuilder builder)
        {
            var storage = new SqliteTestStorage(Tenants.Default);
            storage.EnsureExists();

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithSingleStorage(c =>
                        c.UseSqliteStorage(x => x.ConfigureStorage(c =>
                            c.WithConnectionString(storage.ConnectionString)))));
        }

        static void ConfigurePostgresSingle(IHostApplicationBuilder builder)
        {
            var storage = new PostgresTestStorage(Tenants.Default);
            storage.EnsureExists();

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithSingleStorage(c =>
                        c.UsePostgresStorage(x => x.ConfigureStorage(c => 
                            c.WithConnectionString(storage.ConnectionString)))));
        }

        static void ConfigureSqliteMultitenancy(IHostApplicationBuilder builder)
        {
            var connectionString1 = EnsureStorageExists(new SqliteTestStorage(Tenants.Tenant1));
            var connectionString2 = EnsureStorageExists(new SqliteTestStorage(Tenants.Tenant2));
            var connectionString3 = EnsureStorageExists(new SqliteTestStorage(Tenants.Tenant3));

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithMultitenancy(c => 
                            c.WithTenants(Tenants.Tenant1, Tenants.Tenant2, Tenants.Tenant3)
                             .UseSqliteStorage(x => 
                                    x.WithConnectionString(Tenants.Tenant1, connectionString1)
                                     .WithConnectionString(Tenants.Tenant2, connectionString2)
                                     .WithConnectionString(Tenants.Tenant3, connectionString3))));
        }
        static void ConfigurePostgresMultitenancy(IHostApplicationBuilder builder)
        {
            var connectionString1 = EnsureStorageExists(new PostgresTestStorage(Tenants.Tenant1));
            var connectionString2 = EnsureStorageExists(new PostgresTestStorage(Tenants.Tenant2));
            var connectionString3 = EnsureStorageExists(new PostgresTestStorage(Tenants.Tenant3));

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithMultitenancy(c =>
                            c.WithTenants(Tenants.Tenant1, Tenants.Tenant2, Tenants.Tenant3)
                             .UsePostgresStorage(x =>
                                    x.WithConnectionString(Tenants.Tenant1, connectionString1)
                                     .WithConnectionString(Tenants.Tenant2, connectionString2)
                                     .WithConnectionString(Tenants.Tenant3, connectionString3))));
        }

        static string EnsureStorageExists(ISqlTestStorage storage)
        {
            var result = storage.EnsureExists();
            if (result == false)
            {
                throw new InvalidOperationException($"Failed to create storage {storage.ConnectionString}");
            }

            return storage.ConnectionString;
        }
    }
}
