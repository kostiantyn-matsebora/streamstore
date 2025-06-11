using StreamStore.Testing.StreamStorage;
using StreamStore.InMemory.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.InMemory.Tests.StreamStorage
{
    public class InMemoryTestEnvironment : StorageTestEnvironmentBase
    {
        public InMemoryTestEnvironment()
        {
        }

        protected override void ConfigureStorage(IServiceCollection services)
        {
            services.UseInMemoryStorage();
        }
    }
}
