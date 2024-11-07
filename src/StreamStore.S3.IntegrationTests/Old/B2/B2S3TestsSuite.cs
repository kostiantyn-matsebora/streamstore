using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.S3.B2;
using StreamStore.S3.Client;
using StreamStore.Testing.Framework;


namespace StreamStore.S3.IntegrationTests.Old.B2
{
    public class B2S3TestsSuite : TestSuiteBase, IS3Suite
    {
        public B2S3TestsSuite()
        {
        }

        public IS3LockFactory? CreateLockFactory()
        {
            return Services.GetRequiredService<IS3LockFactory>();
        }

        public IS3ClientFactory? CreateClientFactory()
        {
            return Services.GetRequiredService<IS3ClientFactory>();
        }

        public async Task WithDatabase(Func<IStreamDatabase, Task> action)
        {
            var database = Services.GetRequiredService<IStreamDatabase>();
            await action(database!);
        }

        protected override bool CheckPrerequisities()
        {
            var config = GetConfiguration();
            if (config == null) return false;
            return config.GetSection("b2").Exists();
        }

        protected override void RegisterServices(IServiceCollection services)
        {
            services.UseB2StreamStoreDatabase(GetConfiguration()!);
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
