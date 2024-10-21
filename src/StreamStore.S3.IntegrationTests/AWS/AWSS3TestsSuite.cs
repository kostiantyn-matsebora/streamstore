using Microsoft.Extensions.DependencyInjection;
using StreamStore.S3.AWS;
using StreamStore.S3.Client;
using StreamStore.Testing.Framework;

namespace StreamStore.S3.IntegrationTests.AWS
{
    public class AWSS3TestsSuite : TestSuiteBase, IS3Suite
    {
        public AWSS3TestsSuite()
        { }

        public IS3LockFactory? LockFactory => Services.GetRequiredService<IS3LockFactory>();
        public IS3ClientFactory? ClientFactory => Services.GetRequiredService<IS3ClientFactory>();
        
        public async Task WithDatabase(Func<IStreamDatabase, Task> action)
        {
            var database = Services.GetRequiredService<IStreamDatabase>();
            await action(database!);
        }

        public IS3LockFactory? CreateLockFactory()
        {
            return Services.GetRequiredService<IS3LockFactory>();
        }

        public IS3ClientFactory? CreateClientFactory()
        {
            return Services.GetRequiredService<IS3ClientFactory>();
        }

        protected override bool CheckPrerequisities()
        {
            return File.Exists(Path.Combine(AppContext.BaseDirectory, "appsettings.Development.json"));
        }

        protected override void RegisterServices(IServiceCollection services)
        {
            services.UseS3AmazonStreamStoreDatabase();

        }
    }
}
