using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using StreamStore.ExampleBase;
using StreamStore.Sql.Sqlite;
using StreamStore.Sql.PostgreSql;
using StreamStore.Sql.Tests.PostgreSql.Database;
using StreamStore.Sql.Tests.Sqlite.Database;
using System.CommandLine;

namespace StreamStore.Sql.Example
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        const string databaseName = "streamstore";

        static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            await builder
                .ConfigureExampleApplication(c =>
                    c.EnableMultitenancy() 
                     .AddDatabase(Databases.SQLite, x => x.WithSingleMode(UseSqliteDatabase))
                     .AddDatabase(Databases.Postgres,x => x.WithSingleMode(UsePostgresDatabase)))
                .InvokeAsync(args);
        }

        static void UseSqliteDatabase(IHostApplicationBuilder builder)
        {
            var database = new SqliteTestDatabase(databaseName);
            database.EnsureExists();

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithSingleDatabse(c =>
                        c.UseSqliteDatabase(x => x.ConfigureDatabase(c =>
                            c.WithConnectionString(database.ConnectionString)))));
        }

        static void UsePostgresDatabase(IHostApplicationBuilder builder)
        {
            var database = new PostgresTestDatabase(databaseName);
            database.EnsureExists();

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithSingleDatabse(c =>
                        c.UsePostgresDatabase(x => x.ConfigureDatabase(c => 
                            c.WithConnectionString(database.ConnectionString)))));
        }
    }
}
