using Moq;
using StreamStore.Multitenancy;
using StreamStore.Testing;
using StreamStore.Testing.Framework;
using StreamStore.Validation;

namespace StreamStore.Tests.StreamStore.Multitenancy
{
    public class TenantStreamStoreFactoryTestEnvironment : TestEnvironmentBase
    {
        public static Mock<ITenantStreamStorageProvider> MockTenantStreamStorageProvider => Generated.Mocks.Single<ITenantStreamStorageProvider>();
        public static Mock<IEventConverter> MockEventConverter => Generated.Mocks.Single<IEventConverter>();
        public static Mock<IStreamMutationRequestValidator> MockStreamMutationRequestValidator => Generated.Mocks.Single<IStreamMutationRequestValidator>();

        public static StreamStoreConfiguration Configuration => new StreamStoreConfiguration();
    }
}
