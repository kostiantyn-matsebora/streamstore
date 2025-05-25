using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using StreamStore.ExampleBase.Progress.Model;
using StreamStore.Exceptions;
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

                await store.BeginWriteAsync(streamId, revision, token)
                                .AppendAsync(CreateEvent(), token)
                                .AppendAsync(CreateEvent(), token)
                                .AppendAsync(CreateEvent(), token)
                            .SaveChangesAsync(token);

                TrackProgress(new WriteSucceeded(revision, 3));
            }
            catch (ConcurrencyException ex)
            {
                TrackError(ex);
                if (token.IsCancellationRequested) return;
            }
            finally
            {
                var metadata = await store.GetMetadataAsync(streamId, token);
                revision = metadata != null ? metadata.Revision : Revision.Zero;
            }
        }

        static TestEventEnvelope CreateEvent()
        {
            var fixture = new Fixture();
            return new TestEventEnvelope
            {
                Id = fixture.Create<Id>(),
                Timestamp = fixture.Create<DateTime>(),
                Event = new EventExample
                {
                    InvoiceId = fixture.Create<Guid>(),
                    Name = fixture.Create<string>(),
                    Number = fixture.Create<int>(),
                    Date = fixture.Create<DateTime>()
                }
            };
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
