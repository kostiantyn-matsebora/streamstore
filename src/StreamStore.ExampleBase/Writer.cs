using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using StreamStore.ExampleBase.Progress;
using StreamStore.Exceptions;

namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    internal class Writer : WorkerBase
    {
        Revision actualRevision = Revision.Zero;
        protected override int InitialSleepPeriod => 0;
        readonly WriteProgressTracker progressTracker;

        public Writer(IStreamStore store, Id streamId, WriteProgressTracker progressTracker) : base(store, streamId, progressTracker)
        {
            this.progressTracker = progressTracker;

        }

        protected override async Task DoWorkAsync(CancellationToken token)
        {
            try
            {
                progressTracker.StartWriting();
                actualRevision =
                await store.BeginWriteAsync(streamId, actualRevision, token)
                                .AppendEventAsync(CreateEvent(), token)
                                .AppendEventAsync(CreateEvent(), token)
                                .AppendEventAsync(CreateEvent(), token)
                            .CommitAsync(token);
                progressTracker.WriteSucceeded(actualRevision, 3);

            }
            catch (OptimisticConcurrencyException ex)
            {
                if (token.IsCancellationRequested) return;

                if (ex.ActualRevision != null)
                {
                    actualRevision = ex.ActualRevision!.Value;
                    progressTracker.WriteFailed(actualRevision, ex.Message);
                }
            }
            catch (StreamLockedException ex)
            {
                if (token.IsCancellationRequested) return;
                progressTracker.WriteFailed(actualRevision, ex.Message);
            }
        }

        static Event CreateEvent()
        {
            var fixture = new Fixture();
            return new Event
            {
                Id = fixture.Create<Id>(),
                Timestamp = fixture.Create<DateTime>(),
                EventObject = new EventExample
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
