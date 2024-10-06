
using AutoFixture;
using FluentAssertions;
using Moq;
using StreamStore.InMemory;
using StreamStore.Serialization;

namespace StreamStore.Tests
{
    public class StreamStoreTests
    {
        readonly Mock<IStreamDatabase> mockStreamDatabase;
        readonly IEventSerializer serializer;
        StreamStore streamStore;

        public StreamStoreTests()
        {
            mockStreamDatabase = new Mock<IStreamDatabase>();
            serializer = new EventSerializer();
            streamStore = new StreamStore(mockStreamDatabase.Object, serializer);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDeleteAsyncOnEventDatabase()
        {
            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<string>();

            // Act
            await streamStore.DeleteAsync(streamId);

            // Assert
            mockStreamDatabase.Verify(db => db.DeleteAsync(streamId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_ShouldThrowExceptionIfStreamNotFound()
        {
            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<string>();
            mockStreamDatabase.Setup(db => db.FindAsync(streamId, It.IsAny<CancellationToken>())).ReturnsAsync((StreamRecord?)null);

            // Act
            var act = () => streamStore.GetAsync(streamId);

            // Assert
            await  act.Should().ThrowAsync<StreamNotFoundException>();
        }

        [Fact]
        public async Task OpenStreamAsync_ShouldThrowArgumentNullExceptionIfStreamIdIsNull()
        {
            // Arrange
            var expectedRevision = 1;

            // Act
            var act = () => streamStore.OpenStreamAsync(null!, expectedRevision);

            // & Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowArgumentNullExceptionIfStreamIdIsNull()
        {
            // Act
            var act = () => streamStore.DeleteAsync(null!);

            // & Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetAsync_ShouldThrowArgumentNullExceptionIfStreamIdIsNull()
        {
            // Act
            var act = () => streamStore.GetAsync(null!);

            // Assert
            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        public async Task GetAsync_ShouldReturnStreamEntityWithEvents(int count)
        {
            // Arrange
            Fixture fixture = new();
            var streamRecord = new StreamRecord(fixture.Create<string>(), [.. fixture.CreateEventRecords(count)]);

            var streamId = streamRecord.Id;
            var eventCount = streamRecord.Events.Count();
            var eventIds = streamRecord.Events.Select(e => e.Id).ToArray();

            mockStreamDatabase.Setup(db => db.FindAsync(streamId, It.IsAny<CancellationToken>())).ReturnsAsync(streamRecord);

            // Act
            var result = await streamStore.GetAsync(streamId);

            // Assert
            result.Should().NotBeNull();
            result.StreamId.Should().Be(streamId);
            result.Revision.Should().Be(streamRecord.Revision);
            result.EventEntities.Should().HaveCount(eventCount);
            result.EventEntities.Should().BeInAscendingOrder(e => e.Revision);
            result.EventEntities.Select(e => e.EventId).Should().BeEquivalentTo(eventIds);
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldSaveChanges()
        {
            // Arrange
            streamStore = new StreamStore(new InMemoryStreamDatabase());
            
            var eventIds = new List<Id>();
            var fixture = new Fixture() { OmitAutoProperties = false };
            var streamId = fixture.Create<Id>();

            var events = fixture.CreateMany<Event>(3).ToArray();
            eventIds.AddRange(events.Select(e=> e.Id));

            // Act 1
            // First way to append to stream
            var stream = 
                await streamStore.OpenStreamAsync(streamId, CancellationToken.None);

            stream
                .Add(events[0].Id, events[0].Timestamp, events[0].EventObject)
                .Add(events[1].Id, events[1].Timestamp, events[1].EventObject)
                .Add(events[2].Id, events[2].Timestamp, events[2].EventObject);

            await stream.SaveChangesAsync(CancellationToken.None);


            // Act 2
            events = fixture.CreateMany<Event>(5).ToArray();
            eventIds.AddRange(events.Select(e => e.Id));

            // Second way to append to stream
            stream =
              await streamStore.OpenStreamAsync(streamId, 3, CancellationToken.None);

            stream.AddRange(events);

            await stream.SaveChangesAsync(CancellationToken.None);

            // Act 3
            events = fixture.CreateMany<Event>(100).ToArray();
            eventIds.AddRange(events.Select(e => e.Id));

            // Third way to append to stream
            await streamStore
                .OpenStreamAsync(streamId, 8, CancellationToken.None)
                .AddRangeAsync(events)
                .SaveChangesAsync(CancellationToken.None);


            // Getting stream
            var streamEntity = await streamStore.GetAsync(streamId);

            // Assert
            streamEntity.Should().NotBeNull();
            streamEntity.StreamId.Should().Be(streamId);
            streamEntity.Revision.Should().Be(3 + 5 + 100);

            var eventEntities = streamEntity.EventEntities;
            eventEntities.Should().HaveCount(eventIds.Count);
            eventIds.Should().BeEquivalentTo(eventEntities.Select(e => e.EventId));
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldThrowConcurrencyException()
        {
            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<string>();

            var records = fixture.CreateEventRecords(1, 3);
            var streamRecord = new StreamMetadataRecord(streamId, records);

            mockStreamDatabase
                .Setup(db => db.FindMetadataAsync(streamId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(streamRecord);

            // Act
            // Trying to append events with wrong expected revision
            var act = async () =>
                await
                    streamStore
                        .OpenStreamAsync(streamId, 2, CancellationToken.None)
                        .AddRangeAsync(fixture.CreateMany<Event>(3))
                        .SaveChangesAsync(CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<OptimisticConcurrencyException>();
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldThrowDuplicateEventException()
        {
            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<string>();

            var records = fixture.CreateEventRecords(1, 3);
            var streamRecord = new StreamMetadataRecord(streamId, records);

            var events = records.Select(r => new Event
            {
                Id = r.Id,
                Timestamp = r.Timestamp,
                EventObject = fixture.Create<object>()  //does not matter what object is for this test, just to have something not null
            }).ToArray();

            mockStreamDatabase
                .Setup(db => db.FindMetadataAsync(streamId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(streamRecord);

            mockStreamDatabase
                .Setup(db => db.BeginAppend(streamId, 3))
                .Returns(new Mock<IStreamUnitOfWork>().Object);

            // Act
            // Trying to append existing events mixed with new ones, put existing to the end
            var mixedEvents = fixture.CreateMany<Event>(5).Concat(events);
            
            var act = async () =>
                await
                    streamStore
                        .OpenStreamAsync(streamId, 3, CancellationToken.None)
                        .AddRangeAsync(mixedEvents)
                        .SaveChangesAsync(CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicateEventException>();
        }
    }
}

