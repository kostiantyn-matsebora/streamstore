using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using StreamStore.ExampleBase.Progress.Model;
using StreamStore.Exceptions.Appending;
using StreamStore.Exceptions.Reading;
using StreamStore.Testing;

namespace StreamStore.ExampleBase.Workers
{
    [ExcludeFromCodeCoverage]
    internal class Writer : WorkerBase
    {
        Revision revision = Revision.Zero;

        protected override int InitialSleepPeriod => 0;

        public Writer(WriterIdentifier identifier, IStreamStore store, Id streamId) : base(identifier, store, streamId)
        {
        }

        protected override async Task DoWorkAsync(CancellationToken token)
        {

            try
            {
                TrackProgress(new StartWriting(revision));
                var @event = CreateEvent();
                await store.BeginAppendAsync(streamId, revision, token)
                                .AppendAsync(CreateEvent(), token)
                                .AppendAsync(CreateEvent(false), token)
                                .AppendAsync(builder => builder
                                    .WithId(Generated.Primitives.Id)
                                    .Dated(Generated.Primitives.DateTime)
                                    .WithEvent(Generated.Objects.Single<EventExample>())
                                    .WithCustomProperty(Generated.Primitives.String, Generated.Primitives.String)
                                    .WithCustomProperties(Generated.Objects.Single<Dictionary<string, string>>())
                                    , token)
                            .SaveChangesAsync(token);

                TrackProgress(new WriteSucceeded(revision, 3));
            }
            catch (ConcurrencyControlException ex)
            {
                TrackError(ex);
                if (token.IsCancellationRequested) return;
            }
            finally
            {
                try
                {
                    var metadata = await store.GetMetadataAsync(streamId, token);
                    revision = metadata.Revision;
                } catch (StreamNotFoundException ex)
                {
                    TrackError(ex);
                }

            }
        }

        static TestEventEnvelope CreateEvent(bool withCustomProperties = true)
        {
            var fixture = new Fixture();
            var @event =  new TestEventEnvelope
            {
                Id = Generated.Primitives.Id,
                Timestamp = Generated.Primitives.DateTime,
                Event = new EventExample
                {
                    InvoiceId = Generated.Primitives.Guid,
                    Name = Generated.Primitives.String,
                    Number = Generated.Primitives.Int,
                    Date = Generated.Primitives.DateTime,
                },
                CustomProperties = withCustomProperties ? Generated.Objects.Single<Dictionary<string, string>>() : null
            };

            return @event;
        }

        class EventExample
        {
            public Guid InvoiceId { get; set; }
            public string? Name { get; set; }

            public int Number { get; set; }

            public DateTime Date { get; set; }
        }
    }
}
