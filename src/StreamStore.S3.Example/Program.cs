using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using StreamStore.ExampleBase;
using StreamStore.S3.AWS;
using StreamStore.S3.B2;

namespace StreamStore.S3.Example
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Janitor>();

            await builder
                .ConfigureExampleApplication(c => 
                    c.AddDatabase(S3Databases.AWS, x => x.WithSingleMode(UseAWSDatabase))
                     .AddDatabase(S3Databases.B2, x => x.WithSingleMode(UseB2Database)))
                .InvokeAsync(args);
        }

        static void UseAWSDatabase(IHostApplicationBuilder builder)
        {
            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.WithSingleDatabase(x =>
                        x.UseAWSDatabase()));
        }

        static void UseB2Database(IHostApplicationBuilder builder)
        {
            builder
                .Services
                .ConfigureStreamStore(x =>
                    x.WithSingleDatabase(x =>
                        x.UseB2Database(builder.Configuration)));
        }
    }
}