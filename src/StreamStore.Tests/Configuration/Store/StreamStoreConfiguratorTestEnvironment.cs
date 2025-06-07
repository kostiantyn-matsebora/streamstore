using Microsoft.Extensions.DependencyInjection;
using StreamStore.Configuration;
using StreamStore.Testing.Framework;

namespace StreamStore.Tests.Configuration.Store
{
    public class StreamStoreConfiguratorTestEnvironment : TestEnvironmentBase
    {


        public static IStreamStoreConfigurator CreateConfigurator() => new StreamStoreConfigurator();


        public static IServiceCollection CreateServiceCollection() => new ServiceCollection();
    }
}
