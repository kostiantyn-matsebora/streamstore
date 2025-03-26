using Moq;
using StreamStore.Multitenancy;
using StreamStore.Testing;
using StreamStore.Testing.Framework;

namespace StreamStore.Tests.StreamStore.Multitenancy
{
    public class TenantStreamStoreFactorySuite : TestSuiteBase
    {
        public static Mock<ITenantStreamDatabaseProvider> MockTenantStreamDatabaseProvider => Generated.Mocks.Single<ITenantStreamDatabaseProvider>();
        public static Mock<IEventSerializer> MockEventSerializer => Generated.Mocks.Single<IEventSerializer>();
        public static StreamStoreConfiguration Configuration => new StreamStoreConfiguration();
    }
}
