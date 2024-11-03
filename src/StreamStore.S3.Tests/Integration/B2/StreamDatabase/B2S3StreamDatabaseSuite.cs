using Microsoft.Extensions.Configuration;
using StreamStore.S3.B2;
using StreamStore.Testing;
using StreamStore.Testing.StreamDatabase;

namespace StreamStore.S3.Tests.Integration.B2.StreamDatabase
{
    public class B2S3StreamDatabaseSuite : DatabaseSuiteBase
    {
        readonly S3IntegrationFixture? fixture;
        public override MemoryDatabase Container => fixture!.Container;

        public B2S3StreamDatabaseSuite()
        {
        }

        public B2S3StreamDatabaseSuite(S3IntegrationFixture fixture)
        {
           this.fixture = fixture;
        }

        protected override void ConfigureDatabase(IStreamStoreConfigurator configurator)
        {
            configurator.UseB2StreamDatabase(GetConfiguration()!);
        }

        protected override bool CheckPrerequisities()
        {
            var config = GetConfiguration();
            if (config == null) return false;
            return config.GetSection("b2").Exists();
        }

        protected override Task SetUp()
        {
            fixture!.CopyTo(StreamDatabase);
            return Task.CompletedTask;
        }

        static IConfiguration? GetConfiguration()
        {

            if (!File.Exists(
                    Path.Combine(AppContext.BaseDirectory, "appsettings.Development.json")))
                return null;

            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile($"appsettings.Development.json", true)
                .Build();

            return config;
        }

    }
}
