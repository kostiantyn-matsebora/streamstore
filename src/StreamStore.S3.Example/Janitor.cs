using System.Diagnostics.CodeAnalysis;

namespace StreamStore.S3.Example
{
    [ExcludeFromCodeCoverage]
    public class Janitor : BackgroundService
    {
        readonly ILogger<Janitor> logger;
        readonly IStreamStore store;
        const string streamId = "stream-1";

        public Janitor(ILogger<Janitor> logger, IStreamStore store, IHostApplicationLifetime appLifetime)
        {
            this.logger = logger;
            this.store = store;
            appLifetime.ApplicationStopped.Register(OnStopped);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        private void OnStopped() {
            logger.LogInformation("Cleaning up...");
            store.DeleteAsync(streamId, CancellationToken.None).Wait();
        }
    }
}
