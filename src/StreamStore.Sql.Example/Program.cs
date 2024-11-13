using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using StreamStore.ExampleBase;
using StreamStore.Sql.Sqlite;
using StreamStore.Sql.PostgreSql;
using StreamStore.Sql.Tests.PostgreSql.Database;
using StreamStore.Sql.Tests.Sqlite.Database;

namespace StreamStore.Sql.Example
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        const string databaseName = "streamstore";
        static void Main(string[] args)
        {
          

            var builder = Host.CreateApplicationBuilder(args);

            //UseSqliteDatabase(builder); // Uncomment this line to use SQLite database
            UsePostgresDatabase(builder); // Uncomment this line to use PostgreSQL database

            builder.ConfigureExampleApplication();

            var host = builder.Build();
            
            host.Run();
        }

        static void UseSqliteDatabase(HostApplicationBuilder builder)
        {
            Console.WriteLine("Database backend: SQLite");
            var database = new SqliteTestDatabase(databaseName);
            database.EnsureExists();

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithSingleTenant(c =>
                        c.UseSqliteDatabase(x => x.ConfigureDatabase(c =>
                            c.WithConnectionString(database.ConnectionString)))));
        }

        static void UsePostgresDatabase(HostApplicationBuilder builder)
        {
            Console.WriteLine("Database backend: PostgreSQL");
            var database = new PostgresTestDatabase(databaseName);
            database.EnsureExists();

            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.EnableSchemaProvisioning()
                     .WithSingleTenant(c =>
                        c.UsePostgresDatabase(x => x.ConfigureDatabase(c => 
                            c.WithConnectionString(database.ConnectionString)))));
        }
    }
}
