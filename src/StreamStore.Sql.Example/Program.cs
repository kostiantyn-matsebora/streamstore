using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Hosting;
using StreamStore.ExampleBase;
using StreamStore.Sql.Sqlite;

namespace StreamStore.Sql.Example
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        static void Main(string[] args)
        {
            SQLiteConnection.CreateFile("StreamStore.sqlite");

            var builder = Host.CreateApplicationBuilder(args);
          
            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.UseSqliteDatabase(builder.Configuration));

            builder.ConfigureExampleApplication();

            var host = builder.Build();
            host.Run();
        }
    }
}
