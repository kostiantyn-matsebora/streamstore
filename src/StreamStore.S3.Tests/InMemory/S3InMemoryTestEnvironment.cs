using Microsoft.Extensions.DependencyInjection;
using StreamStore.Testing.Framework;
using StreamStore.InMemory;

namespace StreamStore.S3.Tests.InMemory
{
    public class S3InMemoryTestEnvironment : TestEnvironmentBase
    {
        protected override void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IStreamStorage, InMemoryStreamStorage>();
        }
    }
}
