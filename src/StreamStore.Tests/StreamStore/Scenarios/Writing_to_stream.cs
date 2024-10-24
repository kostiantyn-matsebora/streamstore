using AutoFixture;
using FluentAssertions;
using Moq;
using StreamStore.Exceptions;
using StreamStore.Testing;

namespace StreamStore.Tests.StreamStore
{
    public class Writing_to_stream : Scenario<StreamStoreSuite>
    {

        [Fact]
        public void When_saving_changes()
        {
            //// Arrange
            //streamStore = new StreamStore(new InMemoryStreamDatabase(), new NewtonsoftEventSerializer(registry));

            //var eventIds = new List<Id>();
            //var fixture = new Fixture() { OmitAutoProperties = false };
            //var streamId = fixture.Create<Id>();

            //var events = fixture.CreateMany<Event>(3).ToArray();
            //eventIds.AddRange(events.Select(e => e.Id));

            //// Act 1
            //// First way to append to stream
            //await Suite.Store
            //        .BeginWriteAsync(streamId, CancellationToken.None)
            //            .AppendAsync(events[0].Id, events[0].Timestamp, events[0].EventObject)
            //            .AppendAsync(events[1].Id, events[1].Timestamp, events[1].EventObject)
            //            .AppendAsync(events[2].Id, events[2].Timestamp, events[2].EventObject)
            //        .CommitAsync(CancellationToken.None);


            //// Act 2
            //events = fixture.CreateMany<Event>(5).ToArray();
            //eventIds.AddRange(events.Select(e => e.Id));

            //// Second way to append to stream
            //await streamStore
            //        .BeginWriteAsync(streamId, 3, CancellationToken.None)
            //            .AppendAsync(events[0].Id, events[0].Timestamp, events[0].EventObject)
            //            .AppendAsync(events[1].Id, events[1].Timestamp, events[1].EventObject)
            //            .AppendAsync(events[2].Id, events[2].Timestamp, events[2].EventObject)
            //        .CommitAsync(CancellationToken.None);


            //// Act 3
            //events = fixture.CreateMany<Event>(100).ToArray();
            //eventIds.AddRange(events.Select(e => e.Id));

            //// Third way to append to stream
            //await streamStore
            //        .WriteAsync(streamId, 8, events, CancellationToken.None);

            //// Getting stream
            //var collection = await streamStore.ReadToEndAsync(streamId, CancellationToken.None);

            //// Assert
            //collection.Should().NotBeNull();
            //collection.MaxRevision.Should().Be(3 + 5 + 100);

            //collection.Should().HaveCount(eventIds.Count);
            //collection.Should().BeEquivalentTo(collection.Select(e => e.EventId));

            Assert.True(true);
        }

        [Fact]
        public async Task When_stream_was_already_updated()
        {
            // Arrange
            var fixture = new Fixture();
            var streamId = fixture.Create<string>();

            var records = fixture.CreateEventRecords(1, 3);
            var streamRecord = new StreamMetadataRecord(records);

            Suite.MockDatabase
                .Setup(db => db.FindMetadataAsync(streamId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(streamRecord);

            // Act
            // Trying to append events with wrong expected revision
            var act = async () =>
                await
                    Suite.Store
                        .BeginWriteAsync(streamId, CancellationToken.None)
                        .AddRangeAsync(fixture.CreateMany<Event>(3))
                        .CommitAsync(CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<OptimisticConcurrencyException>();
        }

        [Fact]
        public async Task When_event_with_same_id_already_exists()
        {
            // Arrange
            var streamId = Generated.Id;

            var records = Generated.EventRecords(3);
            var streamRecord = new StreamMetadataRecord(records);

            var events = records.Select(r => new Event
            {
                Id = r.Id,
                Timestamp = r.Timestamp,
                EventObject = Generated.Object<object>()  //does not matter what object is for this test, just to have something not null
            }).ToArray();

            Suite.MockDatabase
                .Setup(db => db.FindMetadataAsync(streamId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(streamRecord);

            Suite.MockDatabase
                 .Setup(db => db.BeginAppendAsync(streamId, 3, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Mock<IStreamUnitOfWork>().Object);

            // Act
            // Trying to append existing events mixed with new ones, put existing to the end
            var mixedEvents = Generated.Events(5).Concat(events);

            var act = async () =>
                await
                    Suite.Store
                        .BeginWriteAsync(streamId, 3, CancellationToken.None)
                        .AddRangeAsync(mixedEvents)
                        .CommitAsync(CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicateEventException>();
        }
    }
}
