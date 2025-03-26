using Microsoft.Extensions.DependencyInjection;
using Moq;
using StreamStore.Configuration.Database;
using StreamStore.Multitenancy;
using StreamStore.Provisioning;
using StreamStore.Testing;
using StreamStore.Testing.Framework;
using StreamStore.Tests.Configuration.SingleTenant;
namespace StreamStore.Tests.Configuration.MultiTenant
{
    public class MultiTenantConfiguratorSuite : TestSuiteBase
    {
        public static IMultitenancyConfigurator CreateConfigurator() => new MultitenancyConfigurator();


        public static IServiceCollection CreateServiceCollection() => new Microsoft.Extensions.DependencyInjection.ServiceCollection();

        public static Mock<IStreamDatabase> MockStreamDatabase => Generated.Mocks.Single<IStreamDatabase>();

        internal class FakeSchemaProvisionerFactory : ITenantSchemaProvisionerFactory
        {
            public ISchemaProvisioner Create(Id tenantId)
            {
                return new SingleTenantConfiguratorSuite.FakeSchemaProvisioner();
            }
        }

        internal class FakeTenantProvider : ITenantProvider
        {
            public IEnumerable<Id> GetAll()
            {
                return Enumerable.Empty<Id>();
            }
        }
    }
}
