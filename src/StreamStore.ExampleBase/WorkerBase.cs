using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling;
using StreamStore.Exceptions;


namespace StreamStore.S3.Example
{
    [ExcludeFromCodeCoverage]
    public class WorkerBase : BackgroundService
    {
        private readonly ILogger<WorkerBase> logger;
        private readonly IStreamStore store;
        public const string StreamId = "stream-1";
        Revision actualRevision = Revision.Zero;

        public WorkerBase(ILogger<WorkerBase> logger, IStreamStore store)
        {
            this.logger = logger;
            this.store = store;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
         
            while (!stoppingToken.IsCancellationRequested)
            {
                var profiler = MiniProfiler.StartNew("Example");
                IStreamWriter stream;
                using (profiler.Step("Main Work"))
                {
                    try
                    {
                        stream = await OpenStreamAsync(stoppingToken);
                        if (stoppingToken.IsCancellationRequested) break;

                        await AddEventsToStreamAsync(stream, stoppingToken);
                        if (stoppingToken.IsCancellationRequested) break;

                        await SaveStreamChangesAsync(stream, stoppingToken);
                        if (stoppingToken.IsCancellationRequested) break;
                    }
                    catch (OptimisticConcurrencyException ex)
                    {
                        HandleOptimisticConcurrencyException(ex, stoppingToken);
                    }
                    catch (StreamLockedException ex)
                    {
                        HandleStreamLockedException(ex, stoppingToken);
                    }
                    finally
                    {
                        await profiler!.StopAsync();
                        logger.LogDebug(profiler.RenderPlainText());
                    }
                }
                logger.LogInformation("Waiting 3 seconds before next iteration.");
                await Task.Delay(3000, stoppingToken);
            }
        }

        async Task<IStreamWriter> OpenStreamAsync(CancellationToken stoppingToken)
        {
            logger.LogDebug("Opening stream of revision {actualRevision}", actualRevision);
            return await store.BeginWriteAsync(StreamId, stoppingToken);
        }

        async Task AddEventsToStreamAsync(IStreamWriter stream, CancellationToken stoppingToken)
        {
            logger.LogDebug("Adding events to stream");

            await stream
                .AppendAsync(CreateEvent(), stoppingToken)
                .AppendAsync(CreateEvent(), stoppingToken)
                .AppendAsync(CreateEvent(), stoppingToken);
        }

        async Task SaveStreamChangesAsync(IStreamWriter stream, CancellationToken stoppingToken)
        {
            logger.LogDebug("Saving changes");

            actualRevision = await stream.CommitAsync(stoppingToken);

            logger.LogInformation("New version of stream: {actualRevision}", actualRevision);
            logger.LogInformation("Congratulations! You have successfully written and read from the stream.");
        }

        void HandleOptimisticConcurrencyException(OptimisticConcurrencyException ex, CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested) return;
            logger.LogWarning("Optimistic concurrency exception: {errorMessage}", ex.Message);
            logger.LogDebug("Trying to get latest version of the stream from exception.");

            if (ex.ActualRevision != null)
            {
                actualRevision = ex.ActualRevision!.Value;
                logger.LogInformation("We've got lucky, actual revision: {actualRevision}", ex.ActualRevision);
            }
        }

        void HandleStreamLockedException(StreamLockedException ex, CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested) return;
            logger.LogWarning("Pessimistic concurrency exception: {errorMessage}", ex.Message);
        }

        static Event CreateEvent()
        {
            return new Event
            {
                Id = Guid.NewGuid().ToString(),
                Timestamp = DateTime.Now,
                EventObject = new EventExample
                {
                    InvoiceId = Guid.NewGuid(),
                    Name = Guid.NewGuid().ToString(),
                    Number = 1,
                    Date = DateTime.Now
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

