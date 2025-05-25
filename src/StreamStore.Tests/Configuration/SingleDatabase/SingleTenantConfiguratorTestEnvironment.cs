using Microsoft.Extensions.DependencyInjection;
using Moq;
using StreamStore.Configuration.Storage;
using StreamStore.Provisioning;
using StreamStore.Testing;
using StreamStore.Testing.Framework;

namespace StreamStore.Tests.Configuration.SingleTenant
{
    public class SingleTenantConfiguratorTestEnvironment: TestEnvironmentBase
    {
        public static ISingleTenantConfigurator CreateConfigurator() => new SingleTenantConfigurator();


        public static IServiceCollection CreateServiceCollection() => new ServiceCollection();

        public static Mock<IStreamStorage> MockStreamStorage => Generated.Mocks.Single<IStreamStorage>();

        internal class FakeSchemaProvisioner : ISchemaProvisioner
        {
            public Task ProvisionSchemaAsync(CancellationToken token)
            {
                return Task.CompletedTask;
            }
        }
    }
}
