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

                
                try
                {
                    logger.LogDebug("Opening stream.");
                    if (stoppingToken.IsCancellationRequested) break;
                    stream = await OpenStream(0, stoppingToken);
                }
                catch (OptimisticConcurrencyException ex)
                {
                    if (stoppingToken.IsCancellationRequested)
                        break;
                    logger.LogWarning("Optimistic concurrency exception: {0}", ex.Message);
                    logger.LogDebug("Trying to get latest version of the stream from exception.");

                    if (stoppingToken.IsCancellationRequested) break;
                    int actualRevision = await GetActualRevision(ex, stoppingToken);

                    if (stoppingToken.IsCancellationRequested) break;
                    stream = await OpenStream(actualRevision, stoppingToken);

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
                entity = await GetStream(stoppingToken);

                logger.LogInformation("New version of stream: {0}", entity.Revision);
                logger.LogInformation("Congratulations! You have successfully written and read from the stream.");
                logger.LogInformation("Waiting 3 seconds before next iteration.");

                await Task.Delay(3000, stoppingToken);
            }
        }

        private async Task<StreamEntity> GetStream(CancellationToken stoppingToken)
        {
          logger.LogInformation("Getting stream");

           return await store.GetAsync(streamId, stoppingToken);
        }

        private async Task<IStream> OpenStream(int actualRevision, CancellationToken stoppingToken)
        {
            logger.LogInformation("Current version of stream: {0}", actualRevision);
            logger.LogDebug("Opening stream with latest version.");

            return await store.OpenStreamAsync(streamId, actualRevision, stoppingToken);
        }

        private async Task<int> GetActualRevision(OptimisticConcurrencyException ex, CancellationToken stoppingToken)
        {
            if (ex.ActualRevision != null)
            {
                logger.LogDebug("Trying to get latest version of the stream from exception.");
                logger.LogInformation("We've got lucky, actual revision: {0}", ex.ActualRevision);
               return ex.ActualRevision.Value;
            }
            else
            {
                logger.LogDebug("Getting new version of stream");
                var entity = await store.GetAsync(streamId, stoppingToken);
                return entity.Revision;
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await DeleteStream();

            logger.LogInformation("Stream is deleted. Or not? Lets check");
            logger.LogDebug("Trying to get stream");

            try
            {
                var stream = await GetStream(CancellationToken.None);
                logger.LogError("Stream is not deleted. Stream revision: {0}", stream.Revision);
            }
            catch (StreamNotFoundException)
            {
                logger.LogInformation("Stream is deleted. Congratulations");
            }

            await base.StopAsync(CancellationToken.None);
        }

        private async Task DeleteStream()
        {
            logger.LogInformation("Trying to delete stream.");

            await store.DeleteAsync(streamId, CancellationToken.None);
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

