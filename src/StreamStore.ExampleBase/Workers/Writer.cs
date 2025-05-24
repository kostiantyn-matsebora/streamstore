using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        Revision actualRevision = Revision.Zero;
        protected override int InitialSleepPeriod => 0;

        public Writer(WriterIdentifier identifier, IStreamStore store, Id streamId) : base(identifier, store, streamId)
        {
        }

        protected override async Task DoWorkAsync(CancellationToken token)
        {

            try
            {
                TrackProgress(new StartWriting(actualRevision));

                await store.BeginWriteAsync(streamId, actualRevision, token)
                                .AppendAsync(CreateEvent(), token)
                                .AppendAsync(CreateEvent(), token)
                                .AppendAsync(CreateEvent(), token)
                            .SaveChangesAsync(token);

                actualRevision = actualRevision + 3;
                TrackProgress(new WriteSucceeded(actualRevision, 3));
            }
            catch (AppendingException ex)
            {
                TrackError(ex);
                if (token.IsCancellationRequested) return;
                actualRevision = await GetActualRevision(token);
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

        async Task<int>  GetActualRevision(CancellationToken token)
        {
            var stream = await store.ReadToEndAsync(streamId, CancellationToken.None);
            return stream!= null && stream.Any() ? stream.Last().Revision:  (int)Revision.Zero;
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
