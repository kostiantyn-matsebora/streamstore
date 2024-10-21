using Microsoft.Extensions.DependencyInjection;
using StreamStore.InMemory;
using StreamStore.Testing.Framework;

namespace StreamStore.Tests.InMemory
{
    public class InMemoryTestSuite : TestSuiteBase
    {
        protected override void RegisterServices(IServiceCollection services)
        {
            services.AddInMemoryStreamDatabase();
        }
    }
}
