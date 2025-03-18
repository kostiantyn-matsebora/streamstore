using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using StreamStore.ExampleBase;
using StreamStore.Sql.Sqlite;
using StreamStore.Sql.PostgreSql;
using StreamStore.Sql.Tests.PostgreSql.Database;
using StreamStore.Sql.Tests.Sqlite.Database;
using System.CommandLine;
using StreamStore.Sql.Tests.Database;

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
                     .AddDatabase(SqlDatabases.SQLite,
                                  x => 
                                    x.WithSingleMode(ConfigureSqliteSingle)
                                     .WithMultitenancy(ConfigureSqliteMultitenancy))
                     .AddDatabase(SqlDatabases.PostgreSQL,
                                   x => 
                                       x.WithSingleMode(ConfigurePostgresSingle)
                                        .WithMultitenancy(ConfigurePostgresMultitenancy)))
                .InvokeAsync(args);
        }

        static void ConfigureSqliteSingle(IHostApplicationBuilder builder)
        {
            var database = new SqliteTestDatabase(Tenants.Default);
            database.EnsureExists();

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithSingleDatabase(c =>
                        c.UseSqliteDatabase(x => x.ConfigureDatabase(c =>
                            c.WithConnectionString(database.ConnectionString)))));
        }

        static void ConfigurePostgresSingle(IHostApplicationBuilder builder)
        {
            var database = new PostgresTestDatabase(Tenants.Default);
            database.EnsureExists();

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithSingleDatabase(c =>
                        c.UsePostgresDatabase(x => x.ConfigureDatabase(c => 
                            c.WithConnectionString(database.ConnectionString)))));
        }

        static void ConfigureSqliteMultitenancy(IHostApplicationBuilder builder)
        {
            var connectionString1 = EnsureDatabaseExists(new SqliteTestDatabase(Tenants.Tenant1));
            var connectionString2 = EnsureDatabaseExists(new SqliteTestDatabase(Tenants.Tenant2));
            var connectionString3 = EnsureDatabaseExists(new SqliteTestDatabase(Tenants.Tenant3));

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithMultitenancy(c => 
                            c.WithTenants(Tenants.Tenant1, Tenants.Tenant2, Tenants.Tenant3)
                             .UseSqliteDatabase(x => 
                                    x.WithConnectionString(Tenants.Tenant1, connectionString1)
                                     .WithConnectionString(Tenants.Tenant2, connectionString2)
                                     .WithConnectionString(Tenants.Tenant3, connectionString3))));
        }
        static void ConfigurePostgresMultitenancy(IHostApplicationBuilder builder)
        {
            var connectionString1 = EnsureDatabaseExists(new PostgresTestDatabase(Tenants.Tenant1));
            var connectionString2 = EnsureDatabaseExists(new PostgresTestDatabase(Tenants.Tenant2));
            var connectionString3 = EnsureDatabaseExists(new PostgresTestDatabase(Tenants.Tenant3));

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithMultitenancy(c =>
                            c.WithTenants(Tenants.Tenant1, Tenants.Tenant2, Tenants.Tenant3)
                             .UsePostgresDatabase(x =>
                                    x.WithConnectionString(Tenants.Tenant1, connectionString1)
                                     .WithConnectionString(Tenants.Tenant2, connectionString2)
                                     .WithConnectionString(Tenants.Tenant3, connectionString3))));
        }

        static string EnsureDatabaseExists(ISqlTestDatabase database)
        {
            var result = database.EnsureExists();
            if (result == false)
            {
                throw new InvalidOperationException($"Failed to create database {database.ConnectionString}");
            }

            return database.ConnectionString;
        }
    }
}
