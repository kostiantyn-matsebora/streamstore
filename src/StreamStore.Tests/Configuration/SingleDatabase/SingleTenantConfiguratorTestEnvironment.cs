using Microsoft.Extensions.DependencyInjection;
using Moq;
using StreamStore.Configuration.Database;
using StreamStore.Provisioning;
using StreamStore.Testing;
using StreamStore.Testing.Framework;

namespace StreamStore.Tests.Configuration.SingleTenant
{
    public class SingleTenantConfiguratorTestEnvironment: TestEnvironmentBase
    {
        public static ISingleTenantConfigurator CreateConfigurator() => new SingleTenantConfigurator();


        public static IServiceCollection CreateServiceCollection() => new Microsoft.Extensions.DependencyInjection.ServiceCollection();

        public static Mock<IStreamStorage> MockStreamDatabase => Generated.Mocks.Single<IStreamStorage>();

        internal class FakeSchemaProvisioner : ISchemaProvisioner
        {
            public Task ProvisionSchemaAsync(CancellationToken token)
            {
                return Task.CompletedTask;
            }
        }
    }
}
