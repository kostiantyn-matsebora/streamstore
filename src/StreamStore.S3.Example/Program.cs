using System.Diagnostics.CodeAnalysis;
using StreamStore.S3.AWS;

namespace StreamStore.S3.Example
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddHostedService<Janitor>();
            builder.Services.AddHostedService<Worker>();
            //builder.Services.AddHostedService<Worker2>();
            //builder.Services.AddHostedService<Worker3>();

            builder
                .Services
                .ConfigureStreamStore()
                .UseS3AmazonStreamStoreDatabase(); // Uncomment this line to use AWS
            //.UseB2StreamStoreDatabase(builder.Configuration); // Uncomment this line to use B2
            var host = builder.Build();
            host.Run();

        }

    }
}