using StreamStore.S3.B2;

namespace StreamStore.S3.Example
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddHostedService<Worker>();

            builder
                .Services
                .ConfigureStreamStore()
                .UseB2StreamStoreDatabase(builder.Configuration);
            var host = builder.Build();
            host.Run();
        }
    }
}