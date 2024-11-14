using Microsoft.Extensions.DependencyInjection;
using Moq;
using StreamStore.Configuration.Database;
using StreamStore.Provisioning;
using StreamStore.Testing;
using StreamStore.Testing.Framework;

namespace StreamStore.Tests.Configuration.SingleTenant
{
    public class SingleTenantConfiguratorSuite: TestSuiteBase
    {
        public static ISingleTenantConfigurator CreateConfigurator() => new SingleTenantConfigurator();


        public static IServiceCollection CreateServiceCollection() => new Microsoft.Extensions.DependencyInjection.ServiceCollection();

        public static Mock<IStreamDatabase> MockStreamDatabase => Generated.MockOf<IStreamDatabase>();

        internal class FakeSchemaProvisioner : ISchemaProvisioner
        {
            public Task ProvisionSchemaAsync(CancellationToken token)
            {
                return Task.CompletedTask;
            }
        }
    }
}
