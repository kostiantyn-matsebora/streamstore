namespace StreamStore.S3.Example
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;
        private readonly IStreamStore store;
        private readonly string streamId = "stream-1";
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
                StreamEntity entity;

                logger.LogDebug("Opening stream.");
                try
                {
                    if (stoppingToken.IsCancellationRequested)
                        break;

                    stream = await store.OpenStreamAsync(streamId, stoppingToken);
                }
                catch (OptimisticConcurrencyException ex)
                {
                    if (stoppingToken.IsCancellationRequested)
                        break;
                    logger.LogWarning("Optimistic concurrency exception: {0}", ex.Message);
                    logger.LogDebug("Trying to get latest version of the stream from exception.");

                    int actualRevision = 0;
                    if (ex.ActualRevision != null)
                    {
                        logger.LogDebug("Trying to get latest version of the stream from exception.");
                        logger.LogInformation("We've got lucky, actual revision: {0}", ex.ActualRevision);
                        actualRevision = ex.ActualRevision.Value;
                    } else
                    {
                        logger.LogDebug("Getting new version of stream");
                        entity = await store.GetAsync(streamId, stoppingToken);
                        actualRevision = entity.Revision;
                    }

                    entity = await store.GetAsync(streamId, stoppingToken);

                    logger.LogInformation("Current version of stream: {0}", actualRevision);
                    logger.LogDebug("Opening stream with latest version.");

                    if (stoppingToken.IsCancellationRequested) break;
                    stream = await store.OpenStreamAsync(streamId, entity.Revision, stoppingToken);

                }

                logger.LogDebug("Adding events to stream");

                stream
                    .Add(CreateEvent())
                    .Add(CreateEvent())
                    .Add(CreateEvent());

                if (stoppingToken.IsCancellationRequested) break;
                logger.LogDebug("Saving changes");

                await stream.SaveChangesAsync(stoppingToken);

                if (stoppingToken.IsCancellationRequested) break;
                logger.LogInformation("Reading stream");

                entity = await store.GetAsync(streamId, stoppingToken);

                logger.LogInformation("New version of stream: {0}", entity.Revision);
                logger.LogInformation("Congratulations! You have successfully written and read from the stream.");
                logger.LogInformation("Waiting 3 seconds before next iteration.");

                await Task.Delay(3000, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Trying to delete stream.");

            await store.DeleteAsync(streamId, CancellationToken.None);

            logger.LogInformation("Stream is deleted. Or not? Lets check");
            logger.LogDebug("Trying to get stream");

            try
            {
                var stream = await store.GetAsync(streamId, CancellationToken.None);
                logger.LogError("Stream is not deleted. Stream revision: {0}", stream.Revision);
            }
            catch (StreamNotFoundException)
            {
                logger.LogInformation("Stream is deleted. Congratulations");
            }
            await base.StopAsync(CancellationToken.None);
        }

        Event CreateEvent()
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

