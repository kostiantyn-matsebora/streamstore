using Microsoft.Extensions.DependencyInjection;
using StreamStore.Configuration;
using StreamStore.Testing.Framework;

namespace StreamStore.Tests.Configuration.Store
{
    public class StreamStoreConfiguratorTestEnvironment : TestEnvironmentBase
    {


        public static INewStreamStoreConfigurator CreateConfigurator() => new NewStreamStoreConfigurator();


        public static IServiceCollection CreateServiceCollection() => new ServiceCollection();
    }
}
