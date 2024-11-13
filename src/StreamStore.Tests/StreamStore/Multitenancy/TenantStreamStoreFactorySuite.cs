using Moq;
using StreamStore.Multitenancy;
using StreamStore.Testing;
using StreamStore.Testing.Framework;

namespace StreamStore.Tests.StreamStore.Multitenancy
{
    public class TenantStreamStoreFactorySuite : TestSuiteBase
    {

        public Mock<ITenantStreamDatabaseProvider> MockTenantStreamDatabaseProvider => Generated.MockOf<ITenantStreamDatabaseProvider>();
        public Mock<IEventSerializer> MockEventSerializer => Generated.MockOf<IEventSerializer>();
        public StreamStoreConfiguration Configuration => new StreamStoreConfiguration();
    }
}
