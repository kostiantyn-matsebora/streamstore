
using AutoFixture;
using FluentAssertions;
using Moq;
using StreamStore.Exceptions;
using StreamStore.InMemory;
using StreamStore.Serialization;
using StreamStore.Testing;

namespace StreamStore.Tests
{
    public class StreamStoreTests
    {
        readonly Mock<IStreamDatabase> mockStreamDatabase;
        readonly IEventSerializer serializer;
        StreamStore streamStore;
        readonly TypeRegistry registry;

        public StreamStoreTests()
        {
            registry = new TypeRegistry();
            mockStreamDatabase = new Mock<IStreamDatabase>();
            serializer = new NewtonsoftEventSerializer(registry);
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
        public async Task BeginReadAsync_ShouldThrowExceptionIfStreamNotFound()
        {
            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<string>();
            mockStreamDatabase.Setup(db => db.FindAsync(streamId, It.IsAny<CancellationToken>())).ReturnsAsync((StreamRecord?)null);

            // Act
            var act = async () => await streamStore.BeginReadAsync(streamId, CancellationToken.None);

            // Assert
            await  act.Should().ThrowAsync<StreamNotFoundException>();
        }

        [Fact]
        public async Task BeginWriteAsync_ShouldThrowArgumentNullExceptionIfStreamIdIsNull()
        {
            // Act
            var act = () => streamStore.BeginWriteAsync(null!, CancellationToken.None);

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
        public async Task BeginReadAsync_ShouldThrowArgumentNullExceptionIfStreamIdIsNull()
        {
            // Act
            var act = () => streamStore.BeginReadAsync(null!);

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
            var streamRecord = new StreamRecord( [.. fixture.CreateEventRecords(count)]);

            var streamId = fixture.Create<Id>();
            var eventCount = streamRecord.Events.Count();
            var eventIds = streamRecord.Events.Select(e => e.Id).ToArray();

            mockStreamDatabase.Setup(db => db.FindAsync(streamId, It.IsAny<CancellationToken>())).ReturnsAsync(streamRecord);

            // Act
            var result = await streamStore.ReadToEndAsync(streamId);

            // Assert
            result.Should().NotBeNull();
            result.MaxRevision.Should().Be(streamRecord.Revision);
            result.Should().HaveCount(eventCount);
            result.Should().BeInAscendingOrder(e => e.Revision);
            result.Select(e => e.EventId).Should().BeEquivalentTo(eventIds);
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldSaveChanges()
        {
            // Arrange
            streamStore = new StreamStore(new InMemoryStreamDatabase(), new NewtonsoftEventSerializer(registry));
            
            var eventIds = new List<Id>();
            var fixture = new Fixture() { OmitAutoProperties = false };
            var streamId = fixture.Create<Id>();

            var events = fixture.CreateMany<Event>(3).ToArray();
            eventIds.AddRange(events.Select(e=> e.Id));

            // Act 1
            // First way to append to stream
            await streamStore
                    .BeginWriteAsync(streamId, CancellationToken.None)
                        .AppendAsync(events[0].Id, events[0].Timestamp, events[0].EventObject)
                        .AppendAsync(events[1].Id, events[1].Timestamp, events[1].EventObject)
                        .AppendAsync(events[2].Id, events[2].Timestamp, events[2].EventObject)
                    .CommitAsync(CancellationToken.None);


            // Act 2
            events = fixture.CreateMany<Event>(5).ToArray();
            eventIds.AddRange(events.Select(e => e.Id));

            // Second way to append to stream
            await streamStore
                    .BeginWriteAsync(streamId, 3, CancellationToken.None)
                        .AppendAsync(events[0].Id, events[0].Timestamp, events[0].EventObject)
                        .AppendAsync(events[1].Id, events[1].Timestamp, events[1].EventObject)
                        .AppendAsync(events[2].Id, events[2].Timestamp, events[2].EventObject)
                    .CommitAsync(CancellationToken.None);

         
            // Act 3
            events = fixture.CreateMany<Event>(100).ToArray();
            eventIds.AddRange(events.Select(e => e.Id));

            // Third way to append to stream
            await streamStore
                    .WriteAsync(streamId, 8, events, CancellationToken.None);

            // Getting stream
            var collection = await streamStore.ReadToEndAsync(streamId, CancellationToken.None);

            // Assert
            collection.Should().NotBeNull();
            collection.MaxRevision.Should().Be(3 + 5 + 100);

            collection.Should().HaveCount(eventIds.Count);
            collection.Should().BeEquivalentTo(collection.Select(e => e.EventId));
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldThrowConcurrencyException()
        {
            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<string>();

            var records = fixture.CreateEventRecords(1, 3);
            var streamRecord = new StreamMetadataRecord(records);

            mockStreamDatabase
                .Setup(db => db.FindMetadataAsync(streamId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(streamRecord);

            // Act
            // Trying to append events with wrong expected revision
            var act = async () =>
                await
                    streamStore
                        .BeginWriteAsync(streamId, CancellationToken.None)
                        .AddRangeAsync(fixture.CreateMany<Event>(3))
                        .CommitAsync(CancellationToken.None);

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
            var streamRecord = new StreamMetadataRecord(records);

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
                .Setup(db => db.BeginAppendAsync(streamId, 3, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Mock<IStreamUnitOfWork>().Object);

            // Act
            // Trying to append existing events mixed with new ones, put existing to the end
            var mixedEvents = fixture.CreateMany<Event>(5).Concat(events);
            
            var act = async () =>
                await
                    streamStore
                        .BeginWriteAsync(streamId, 3, CancellationToken.None)
                        .AddRangeAsync(mixedEvents)
                        .CommitAsync(CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicateEventException>();
        }
    }
}

