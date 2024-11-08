using System.Diagnostics.CodeAnalysis;
using StreamStore.ExampleBase;
using StreamStore.S3.AWS;

namespace StreamStore.S3.Example
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder
                .Services
                .ConfigureStreamStore(x => x.UseAWSDatabase());


            builder.ConfigureExampleApplication();
            builder.Services.AddHostedService<Janitor>();

            var host = builder.Build();
            host.Run();

        }

    }
}