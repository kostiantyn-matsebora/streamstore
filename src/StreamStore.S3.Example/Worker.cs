using Microsoft.Extensions.Logging;
using StreamStore.Exceptions;

namespace StreamStore.S3.Example
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IStreamStore store;
        public const string StreamId = "stream-1";
        int actualRevision = 0;

        public Worker(ILogger<Worker> logger, IStreamStore store)
        {
            this.logger = logger;
            this.store = store;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                IStream stream;

                try
                {
                    logger.LogDebug("Opening stream of revision {actualRevision}", actualRevision);
                    if (stoppingToken.IsCancellationRequested) break;
                    logger.LogInformation("Current version of stream: {actualRevision}", actualRevision);
                    logger.LogDebug("Opening stream with latest version.");

                    stream = await store.OpenStreamAsync(StreamId, actualRevision, stoppingToken);

                    logger.LogDebug("Adding events to stream");

                    stream
                        .Add(CreateEvent())
                        .Add(CreateEvent())
                        .Add(CreateEvent());

                    if (stoppingToken.IsCancellationRequested) break;
                    logger.LogDebug("Saving changes");

                    actualRevision = await stream.SaveChangesAsync(stoppingToken);

                    if (stoppingToken.IsCancellationRequested) break;

                    logger.LogInformation("New version of stream: {actualRevision}", actualRevision);
                    logger.LogInformation("Congratulations! You have successfully written and read from the stream.");

                }
                catch (OptimisticConcurrencyException ex)
                {
                    if (stoppingToken.IsCancellationRequested) break;
                    logger.LogWarning("Optimistic concurrency exception: {errorMessage}", ex.Message);
                    logger.LogDebug("Trying to get latest version of the stream from exception.");

                    if (ex.ActualRevision != null)
                    {
                        actualRevision = ex.ActualRevision!.Value;
                        logger.LogInformation("We've got lucky, actual revision: {actualRevision}", ex.ActualRevision);
                    }
                }
                catch (PessimisticConcurrencyException ex)
                {
                    if (stoppingToken.IsCancellationRequested) break;
                    logger.LogWarning("Pessimistic concurrency exception: {errorMessage}", ex.Message);
                }

                logger.LogInformation("Waiting 3 seconds before next iteration.");
                await Task.Delay(3000, stoppingToken);
            }
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

