using Microsoft.Extensions.DependencyInjection;
using StreamStore.InMemory;
using StreamStore.Testing.Framework;

namespace StreamStore.S3.Tests.InMemory
{
    public class S3InMemorySuite : TestSuiteBase
    {
        protected override void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IStreamDatabase, InMemoryStreamDatabase>();
        }
    }
}
