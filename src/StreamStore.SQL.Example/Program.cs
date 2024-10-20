using System.Data.SQLite;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using StreamStore.SQL.Sqlite;

namespace StreamStore.SQL.Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SQLiteConnection.CreateFile("StreamStore.sqlite");

            var builder = Host.CreateApplicationBuilder(args);
            builder.Logging.AddSimpleConsole(configure =>
            {
                configure.SingleLine = true;
                configure.ColorBehavior = LoggerColorBehavior.Enabled;
                configure.IncludeScopes = true;
            });
             

            builder
                .Services
                .ConfigureStreamStore()
                .ConfigureSqliteStreamDatabase()
                    .WithConnectionString("Data Source=StreamStore.sqlite;Version=3;")
                    .WithProfiling()
                .Configure();



            builder.Services.AddHostedService<Worker1>();
            builder.Services.AddHostedService<Worker2>();
            builder.Services.AddHostedService<Worker3>();
            var host = builder.Build();
            host.Run();

        }

    }
}
