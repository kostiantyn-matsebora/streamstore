using FluentAssertions;
using StreamStore.Storage.Multitenancy;
using StreamStore.Testing;

namespace StreamStore.Tests.Multitenancy
{
    public class Provide_storage: Scenario
    {
        [Fact]
        public void When_delegate_is_not_defined()
        {
            var act = () => new DelegateStorageProvider(null!);
            act.Should().Throw<ArgumentNullException>().WithParameterName("provider");
        }

        [Fact]
        public void Should_invoke_delegate_when_creating_provisioner()
        {
            // Arrange
            var tenantId = Generated.Primitives.Id;
            var storage = Generated.Mocks.Single<IStreamStorage>();
            var provider = new DelegateStorageProvider(_ => storage.Object);

            // Act
            var createdStorage = provider.GetStorage(tenantId);

            // Assert
            createdStorage.Should().Be(storage.Object);
        }
    }
}
