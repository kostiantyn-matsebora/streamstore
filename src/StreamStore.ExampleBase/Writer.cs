using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using StopwatchTimer;
using StreamStore.Exceptions;

namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    internal class Writer : WorkerBase
    {
        Revision actualRevision = Revision.Zero;

        protected override int InitialSleepPeriod => 0;

        public Writer(ILogger logger, IStreamStore store, Id streamId) : base(logger, store, streamId)
        {
        }

        protected override async Task DoWorkAsync(int sleepPeriod, CancellationToken token)
        {
            try
            {
                using (new CodeStopWatch("Writing 3 events to stream", s => logger.LogInformation(s)))
                {
                    actualRevision =
                    await store.BeginWriteAsync(streamId, actualRevision, token)
                                    .AppendEventAsync(CreateEvent(), token)
                                    .AppendEventAsync(CreateEvent(), token)
                                    .AppendEventAsync(CreateEvent(), token)
                                .CommitAsync(token);
                }
            }
            catch (OptimisticConcurrencyException ex)
            {
                if (token.IsCancellationRequested) return;

                if (ex.ActualRevision != null)
                {
                    logger.LogWarning(ex.Message);
                    actualRevision = ex.ActualRevision!.Value;
                }
            }
            catch (StreamLockedException ex)
            {
                if (token.IsCancellationRequested) return;
                logger.LogWarning(ex.Message);
            }
            await Task.Delay(sleepPeriod, token);
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
