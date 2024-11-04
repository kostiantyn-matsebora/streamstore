using Microsoft.Extensions.DependencyInjection;
using StreamStore.Testing.Framework;

namespace StreamStore.Tests.Configurator
{
    public class StreamStoreConfiguratorSuite: TestSuiteBase
    {


        public StreamStoreConfigurator CreateConfigurator() => new StreamStoreConfigurator();
        

        public IServiceCollection CreateServiceCollection() => new Microsoft.Extensions.DependencyInjection.ServiceCollection();
    }
}
