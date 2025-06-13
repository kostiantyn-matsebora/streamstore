using EventFlow;
using EventFlow.Core;
using EventFlow.EventStores;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StreamStore.Sql.Sqlite;

namespace StreamStore.Storage.EventFlow.Tests
{
    [TestFixture]
    public class Configure_multitenancy
    {

        [Test]
        public void Should_throw_when_parameters_not_set()
        {
            // Act
            var act = () => EventFlowOptionsExtension.UseStreamStorageEventStore<PredefinedTenantIdResolver>(null!, c => c.UseSqlite());

            // Assert
            act.Should().Throw<ArgumentNullException>();

            // Arrange
            act = () => new Mock<IEventFlowOptions>().Object.UseStreamStorageEventStore<PredefinedTenantIdResolver>(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Should_configure_event_persistence()
        {
            // Arrange 
            var tenantId = Generated.Id;
            var options = new Mock<IEventFlowOptions>();
            var serviceCollection = 
               new ServiceCollection()
                .AddSingleton(new Mock<IJsonSerializer>().Object)
                .AddSingleton(new IdWrapper(tenantId));

            options.Setup(c => c.ServiceCollection).Returns(serviceCollection);

            // Act
            options.Object.UseStreamStorageEventStore<PredefinedTenantIdResolver>(
                     c =>c.UseSqliteWithMultitenancy(c => c.WithConnectionString(tenantId, Generated.String)));

            var provider = serviceCollection.BuildServiceProvider();

            // Assert
            provider.GetService<IStreamStorage>().Should().NotBeNull();
            provider.GetService<IEventPersistence>().Should().NotBeNull().And.BeOfType<StreamStoragePersistence>();

        }
    }

    class PredefinedTenantIdResolver : ITenantIdResolver
    {
        readonly IdWrapper tenantId;
        public PredefinedTenantIdResolver(IdWrapper tenantId)
        {
            this.tenantId = tenantId;
        }

        public Id Resolve()
        {
            return tenantId.Value;
        }
    }

    class IdWrapper
    {
        public IdWrapper(Id id)
        {
            Value = id;
        }
        public Id Value { get; }
    }
}
