using Microsoft.Extensions.DependencyInjection;
using StreamStore.Testing.Framework;

namespace StreamStore.Tests.Configuration.StoreConfigurator
{
    public class StreamStoreConfiguratorSuite : TestSuiteBase
    {


        public static StreamStoreConfigurator CreateConfigurator() => new StreamStoreConfigurator();


        public static IServiceCollection CreateServiceCollection() => new Microsoft.Extensions.DependencyInjection.ServiceCollection();
    }
}
