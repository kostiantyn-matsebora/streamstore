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
        const string singleDatabaseName = "streamstore";
        readonly static Id tenant1 = "tenant-1";
        readonly static Id tenant2 = "tenant-2";
        readonly static Id tenant3 = "tenant-3";

        static Id[] tenants =  new Id[] { "tenant-1", "tenant-2", "tenant-3"};

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
            var database = new SqliteTestDatabase(singleDatabaseName);
            database.EnsureExists();

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithSingleDatabse(c =>
                        c.UseSqliteDatabase(x => x.ConfigureDatabase(c =>
                            c.WithConnectionString(database.ConnectionString)))));
        }

        static void ConfigurePostgresSingle(IHostApplicationBuilder builder)
        {
            var database = new PostgresTestDatabase(singleDatabaseName);
            database.EnsureExists();

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithSingleDatabse(c =>
                        c.UsePostgresDatabase(x => x.ConfigureDatabase(c => 
                            c.WithConnectionString(database.ConnectionString)))));
        }

        static void ConfigureSqliteMultitenancy(IHostApplicationBuilder builder)
        {
            var connectionString1 = EnsureDatabaseExists(new SqliteTestDatabase(tenant1));
            var connectionString2 = EnsureDatabaseExists(new SqliteTestDatabase(tenant2));
            var connectionString3 = EnsureDatabaseExists(new SqliteTestDatabase(tenant3));

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithMultitenancy(c => 
                            c.WithTenants(tenant1, tenant2, tenant3)
                             .UseSqliteDatabase(x => 
                                    x.WithConnectionString(tenant1, connectionString1)
                                     .WithConnectionString(tenant2, connectionString2)
                                     .WithConnectionString(tenant3, connectionString3))));
        }
        static void ConfigurePostgresMultitenancy(IHostApplicationBuilder builder)
        {
            var connectionString1 = EnsureDatabaseExists(new PostgresTestDatabase(tenant1));
            var connectionString2 = EnsureDatabaseExists(new PostgresTestDatabase(tenant2));
            var connectionString3 = EnsureDatabaseExists(new PostgresTestDatabase(tenant3));

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithMultitenancy(c =>
                            c.WithTenants(tenant1, tenant2, tenant3)
                             .UseSqliteDatabase(x =>
                                    x.WithConnectionString(tenant1, connectionString1)
                                     .WithConnectionString(tenant2, connectionString2)
                                     .WithConnectionString(tenant3, connectionString3))));
        }

        static string EnsureDatabaseExists(ITestDatabase database)
        {
            database.EnsureExists();
            return database.ConnectionString;
        }
    }
}
