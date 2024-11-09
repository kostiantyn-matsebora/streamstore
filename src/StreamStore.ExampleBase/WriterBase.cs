using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling;
using StreamStore.Exceptions;


namespace StreamStore.S3.Example
{
    [ExcludeFromCodeCoverage]
    public abstract class WriterBase : BackgroundService
    {
        readonly ILogger<WriterBase> logger;
        readonly IStreamStore store;
        public const string StreamId = "stream-1";
        Revision actualRevision = Revision.Zero;

        protected WriterBase(ILogger<WriterBase> logger, IStreamStore store)
        {
            this.logger = logger;
            this.store = store;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
         
            while (!stoppingToken.IsCancellationRequested)
            {
                    try
                    {
                        actualRevision = 
                            await store.BeginWriteAsync(StreamId, actualRevision, stoppingToken)
                                            .AppendEventAsync(CreateEvent(), stoppingToken)
                                            .AppendEventAsync(CreateEvent(), stoppingToken)
                                            .AppendEventAsync(CreateEvent(), stoppingToken)
                                        .CommitAsync(stoppingToken);
                    }
                    catch (OptimisticConcurrencyException ex)
                    {
                        if (stoppingToken.IsCancellationRequested) return;

                        if (ex.ActualRevision != null)
                        {
                            logger.LogWarning(ex.Message);
                            actualRevision = ex.ActualRevision!.Value;
                        }
                    }
                    catch (StreamLockedException ex)
                    {
                        if (stoppingToken.IsCancellationRequested) return;
                        logger.LogWarning(ex.Message);
                    }
                logger.LogInformation("Waiting 3 seconds before next iteration.");
                await Task.Delay(3000, stoppingToken);
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
    }

    class EventExample
    {
        public Guid InvoiceId { get; set; }
        public string? Name { get; set; }

        public int Number { get; set; }

        public DateTime Date { get; set; }
    }
}

