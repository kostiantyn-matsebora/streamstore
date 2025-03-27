using Microsoft.Extensions.DependencyInjection;
using StreamStore.Testing.Framework;

namespace StreamStore.Tests.Configuration.Store
{
    public class StreamStoreConfiguratorTestEnvironment : TestEnvironmentBase
    {


        public static StreamStoreConfigurator CreateConfigurator() => new StreamStoreConfigurator();


        public static IServiceCollection CreateServiceCollection() => new ServiceCollection();
    }
}
