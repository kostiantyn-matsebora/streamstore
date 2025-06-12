using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.Exceptions;
using EventFlow.Extensions;
using EventFlow.Subscribers;
using EventFlow.TestHelpers;
using EventFlow.TestHelpers.Aggregates;
using EventFlow.TestHelpers.Aggregates.Commands;
using EventFlow.TestHelpers.Aggregates.Entities;
using EventFlow.TestHelpers.Aggregates.Events;
using EventFlow.TestHelpers.Aggregates.ValueObjects;
using EventFlow.TestHelpers.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StreamStore.Provisioning;

namespace StreamStore.EventFlow.Tests
{

    public abstract class TestSuiteForEventStore: IntegrationTest
    {
        readonly List<IDomainEvent> publishedDomainEvents = new List<IDomainEvent>();


        [Test]
        public async Task NewAggregateCanBeLoaded()
        {
            // Act
            var testAggregate = await LoadAggregateAsync(ThingyId.New);

            // Assert
            testAggregate.Should().NotBeNull();
            testAggregate.IsNew.Should().BeTrue();
        }

        [Test]
        public async Task EventsCanBeStored()
        {
            // Arrange
            var id = ThingyId.New;
            var testAggregate = await LoadAggregateAsync(id);
            testAggregate.Ping(PingId.New);

            // Act
            var domainEvents = await testAggregate.CommitAsync(EventStore, SnapshotStore, SourceId.New, CancellationToken.None);

            // Assert
            domainEvents.Count.Should().Be(1);
            var pingEvent = domainEvents.Single() as IDomainEvent<ThingyAggregate, ThingyId, ThingyPingEvent>;
            pingEvent.Should().NotBeNull();
            pingEvent!.AggregateIdentity.Should().Be(id);
            pingEvent.AggregateSequenceNumber.Should().Be(1);
            pingEvent.AggregateType.Should().Be(typeof(ThingyAggregate));
            pingEvent.EventType.Should().Be(typeof(ThingyPingEvent));
            pingEvent.Timestamp.Should().NotBe(default);
            pingEvent.Metadata.Count.Should().BeGreaterThan(0);
            pingEvent.Metadata.SourceId.IsNone().Should().BeFalse();
        }

        [Test]
        public async Task AggregatesCanBeLoaded()
        {
            // Arrange
            var id = ThingyId.New;
            var testAggregate = await LoadAggregateAsync(id);
            testAggregate.Ping(PingId.New);
            await testAggregate.CommitAsync(EventStore, SnapshotStore, SourceId.New, CancellationToken.None);

            // Act
            var loadedTestAggregate = await LoadAggregateAsync(id);

            // Assert
            loadedTestAggregate.Should().NotBeNull();
            loadedTestAggregate.IsNew.Should().BeFalse();
            loadedTestAggregate.Version.Should().Be(1);
            loadedTestAggregate.PingsReceived.Count.Should().Be(1);
        }

        [Test]
        public async Task EventsCanContainUnicodeCharacters()
        {
            // Arrange
            var id = ThingyId.New;
            var testAggregate = await LoadAggregateAsync(id);
            var message = new ThingyMessage(ThingyMessageId.New, "😉");

            testAggregate.AddMessage(message);
            await testAggregate.CommitAsync(EventStore, SnapshotStore, SourceId.New, CancellationToken.None);

            // Act
            var loadedTestAggregate = await LoadAggregateAsync(id);

            // Assert
            loadedTestAggregate.Messages.Single().Message.Should().Be("😉");
        }

        [Test]
        public async Task AggregateEventStreamsAreSeperate()
        {
            // Arrange
            var id1 = ThingyId.New;
            var id2 = ThingyId.New;
            var aggregate1 = await LoadAggregateAsync(id1);
            var aggregate2 = await LoadAggregateAsync(id2);
            aggregate1.Ping(PingId.New);
            aggregate2.Ping(PingId.New);
            aggregate2.Ping(PingId.New);

            // Act
            await aggregate1.CommitAsync(EventStore, SnapshotStore, SourceId.New, CancellationToken.None);
            await aggregate2.CommitAsync(EventStore, SnapshotStore, SourceId.New, CancellationToken.None);
            aggregate1 = await LoadAggregateAsync(id1);
            aggregate2 = await LoadAggregateAsync(id2);

            // Assert
            aggregate1.Version.Should().Be(1);
            aggregate2.Version.Should().Be(2);
        }

        [Test]
        public async Task LoadingOfEventsCanStartLater()
        {
            // Arrange
            var id = ThingyId.New;
            await PublishPingCommandsAsync(id, 5);

            // Act
            var domainEvents = await EventStore.LoadEventsAsync<ThingyAggregate, ThingyId>(id, 3, CancellationToken.None);

            // Assert
            domainEvents.Should().HaveCount(3);
            domainEvents.ElementAt(0).AggregateSequenceNumber.Should().Be(3);
            domainEvents.ElementAt(1).AggregateSequenceNumber.Should().Be(4);
            domainEvents.ElementAt(2).AggregateSequenceNumber.Should().Be(5);
        }

        [Test]
        public async Task LoadingOfEventsCanStartLaterAndStopEarlier()
        {
            // Arrange
            var id = ThingyId.New;
            await PublishPingCommandsAsync(id, 5);

            // Act
            var domainEvents = await EventStore.LoadEventsAsync<ThingyAggregate, ThingyId>(id, 3, 4, CancellationToken.None);

            // Assert
            domainEvents.Should().HaveCount(2);
            domainEvents.ElementAt(0).AggregateSequenceNumber.Should().Be(3);
            domainEvents.ElementAt(1).AggregateSequenceNumber.Should().Be(4);
        }

        [Test]
        public async Task AggregateCanHaveMultipleCommits()
        {
            // Arrange
            var id = ThingyId.New;

            // Act
            var aggregate = await LoadAggregateAsync(id);
            aggregate.Ping(PingId.New);
            await aggregate.CommitAsync(EventStore, SnapshotStore, SourceId.New, CancellationToken.None);
            aggregate = await LoadAggregateAsync(id);
            aggregate.Ping(PingId.New);
            await aggregate.CommitAsync(EventStore, SnapshotStore, SourceId.New, CancellationToken.None);
            aggregate = await LoadAggregateAsync(id);

            // Assert
            aggregate.PingsReceived.Count.Should().Be(2);
        }

        [Test]
        public async Task EventsAreUpgraded()
        {
            // Arrange
            var id = ThingyId.New;
            const int version1 = 3;
            const int version2 = 5;
            const int version3 = 1;

            // Act
            await CommandBus.PublishAsync(
                new ThingyEmitUpgradableEventsCommand(id, version1, version2, version3));

            // Assert
            var aggregate = await LoadAggregateAsync(id);
            aggregate.UpgradableEventV1Received.Should().Be(0);
            aggregate.UpgradableEventV2Received.Should().Be(0);
            aggregate.UpgradableEventV3Received.Should().Be(version1 + version2 + version3);
        }

        [Test]
        public async Task AggregateEventStreamsCanBeDeleted()
        {
            // Arrange
            var id1 = ThingyId.New;
            var id2 = ThingyId.New;
            var aggregate1 = await LoadAggregateAsync(id1);
            var aggregate2 = await LoadAggregateAsync(id2);
            aggregate1.Ping(PingId.New);
            aggregate2.Ping(PingId.New);
            aggregate2.Ping(PingId.New);
            await aggregate1.CommitAsync(EventStore, SnapshotStore, SourceId.New, CancellationToken.None);
            await aggregate2.CommitAsync(EventStore, SnapshotStore, SourceId.New, CancellationToken.None);

            // Act
            await EventStore.DeleteAggregateAsync<ThingyAggregate, ThingyId>(id2, CancellationToken.None);

            // Assert
            aggregate1 = await LoadAggregateAsync(id1);
            aggregate2 = await LoadAggregateAsync(id2);
            aggregate1.Version.Should().Be(1);
            aggregate2.Version.Should().Be(0);
        }

        [Test]
        public async Task NoEventsEmittedIsOk()
        {
            // Arrange
            var id = ThingyId.New;
            var aggregate = await LoadAggregateAsync(id);

            // Act
           var act = async () => await aggregate.CommitAsync(EventStore, SnapshotStore, SourceId.New, CancellationToken.None);

            // Assert 
            await act.Should().NotThrowAsync();
        }

        [Test]
        public async Task OptimisticConcurrency()
        {
            // Arrange
            var id = ThingyId.New;
            var aggregate1 = await LoadAggregateAsync(id);
            var aggregate2 = await LoadAggregateAsync(id);

            aggregate1.DomainErrorAfterFirst();
            aggregate2.DomainErrorAfterFirst();

            // Act
            await aggregate1.CommitAsync(EventStore, SnapshotStore, SourceId.New, CancellationToken.None);
            await ThrowsExceptionAsync<OptimisticConcurrencyException>(() => aggregate2.CommitAsync(EventStore, SnapshotStore, SourceId.New, CancellationToken.None));
        }

        [Test]
        public async Task AggregatesCanUpdatedAfterOptimisticConcurrency()
        {
            // Arrange
            var id = ThingyId.New;
            var pingId1 = PingId.New;
            var pingId2 = PingId.New;
            var aggregate1 = await LoadAggregateAsync(id);
            var aggregate2 = await LoadAggregateAsync(id);
            aggregate1.Ping(pingId1);
            aggregate2.Ping(pingId2);
            await aggregate1.CommitAsync(EventStore, SnapshotStore, SourceId.New, CancellationToken.None);
            await ThrowsExceptionAsync<OptimisticConcurrencyException>(() => aggregate2.CommitAsync(EventStore, SnapshotStore, SourceId.New, CancellationToken.None));

            // Act
            aggregate1 = await LoadAggregateAsync(id);
            aggregate1.PingsReceived.Single().Should().Be(pingId1);
            aggregate1.Ping(pingId2);
            await aggregate1.CommitAsync(EventStore, SnapshotStore, SourceId.New, CancellationToken.None);

            // Assert
            aggregate1 = await LoadAggregateAsync(id);
            aggregate1.PingsReceived.Should().BeEquivalentTo(new[] { pingId1, pingId2 });
        }

        [Test]
        public async Task MultipleScopes()
        {
            // Arrange
            var id = ThingyId.New;
            var pingId1 = PingId.New;
            var pingId2 = PingId.New;

            // Act
            using (var serviceScope = ServiceProvider.CreateScope())
            {
                var commandBus = serviceScope.ServiceProvider.GetRequiredService<ICommandBus>();
                await commandBus.PublishAsync(
                    new ThingyPingCommand(id, pingId1))
                    ;
            }
            using (var serviceScope = ServiceProvider.CreateScope())
            {
                var commandBus = serviceScope.ServiceProvider.GetRequiredService<ICommandBus>();
                await commandBus.PublishAsync(
                        new ThingyPingCommand(id, pingId2))
                    ;
            }

            // Assert
            var aggregate = await LoadAggregateAsync(id);
            aggregate.PingsReceived.Should().BeEquivalentTo(new[] { pingId1, pingId2 });
        }

        [Test]
        public async Task PublishedDomainEventsHaveAggregateSequenceNumbers()
        {
            // Arrange
            var id = ThingyId.New;
            var pingIds = Many<PingId>(10);

            // Act
            await CommandBus.PublishAsync(
                new ThingyMultiplePingsCommand(id, pingIds))
                ;

            // Assert
            publishedDomainEvents.Count.Should().Be(10);
            publishedDomainEvents.Select(d => d.AggregateSequenceNumber).Should().BeEquivalentTo(Enumerable.Range(1, 10));
        }

        [Test]
        public async Task PublishedDomainEventsContinueAggregateSequenceNumbers()
        {
            // Arrange
            var id = ThingyId.New;
            var pingIds = Many<PingId>(10);
            await CommandBus.PublishAsync(
                new ThingyMultiplePingsCommand(id, pingIds))
                ;
            publishedDomainEvents.Clear();

            // Act
            await CommandBus.PublishAsync(
                new ThingyMultiplePingsCommand(id, pingIds))
                ;

            // Assert
            publishedDomainEvents.Count.Should().Be(10);
            publishedDomainEvents.Select(d => d.AggregateSequenceNumber).Should().BeEquivalentTo(Enumerable.Range(11, 10));
        }


        [SetUp]
        public void TestSuiteForEventStoreSetUp()
        {
            publishedDomainEvents.Clear();
        }

        private static async Task ThrowsExceptionAsync<TException>(Func<Task> action)
            where TException : Exception
        {
            var wasCorrectException = false;

            try
            {
                await action();
            }
            catch (Exception e)
            {
                wasCorrectException = e.GetType() == typeof(TException);
                if (!wasCorrectException)
                {
                    throw;
                }
            }

            wasCorrectException.Should().BeTrue("Action was expected to throw exception {0}", typeof(TException).PrettyPrint());
        }
    

        protected override IEventFlowOptions Options(IEventFlowOptions eventFlowOptions)
        {
            Mock<ISubscribeSynchronousToAll> subscribeSynchronousToAllMock = CreateSynchronousSubscriber();
            
            var storageName = Generated.StorageName;
            ProvisionStorage(storageName);

            return base.Options(eventFlowOptions)
                .RegisterServices(sr => sr.AddSingleton(_ => subscribeSynchronousToAllMock.Object))
                .UseStreamStoreEventStore(services => ConfigureStreamStorage(services, storageName));
        }

        protected override IServiceProvider Configure(IEventFlowOptions eventFlowOptions)
        {
            var serviceProvider = eventFlowOptions.ServiceCollection.BuildServiceProvider();
            ProvisionSchema(serviceProvider);
            return serviceProvider;
        }

        static void ProvisionSchema(ServiceProvider serviceProvider)
        {
            var schemaProvisioner = serviceProvider.GetRequiredService<ISchemaProvisioner>();
            schemaProvisioner.ProvisionSchemaAsync(CancellationToken.None).GetAwaiter().GetResult();
        }

        Mock<ISubscribeSynchronousToAll> CreateSynchronousSubscriber()
        {
            var subscribeSynchronousToAllMock = new Mock<ISubscribeSynchronousToAll>();

            subscribeSynchronousToAllMock
                .Setup(s => s.HandleAsync(It.IsAny<IReadOnlyCollection<IDomainEvent>>(), It.IsAny<CancellationToken>()))
                .Callback<IReadOnlyCollection<IDomainEvent>, CancellationToken>((d, c) => publishedDomainEvents.AddRange(d))
                .Returns(Task.FromResult(0));
            return subscribeSynchronousToAllMock;
        }

        protected abstract void ProvisionStorage(string name);
        protected abstract void ConfigureStreamStorage(IServiceCollection services, string storageName);

    }
}