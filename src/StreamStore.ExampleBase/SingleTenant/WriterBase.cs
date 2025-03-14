using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using StreamStore.ExampleBase.Progress;



namespace StreamStore.ExampleBase.SingleTenant
{
    [ExcludeFromCodeCoverage]
    public abstract class WriterBase : BackgroundService
    {
        readonly IStreamStore store;
        readonly WriteProgressTracker progressTracker;
        const string streamId = "stream-1";

        protected WriterBase(IStreamStore store, ProgressTrackerFactory trackerFactory)
        {
            this.store = store;
            this.progressTracker = trackerFactory.SpawnWriteTracker(GetType().Name);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await new Writer(store, streamId, progressTracker).BeginWorkAsync(100, stoppingToken);
        }
    }
}

