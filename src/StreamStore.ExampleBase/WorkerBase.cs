using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using StreamStore.ExampleBase.Progress;


namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    internal abstract class WorkerBase
    {
        protected readonly IStreamStore store;
        protected readonly Id streamId;
        readonly ProgressTracker progressTracker;

        protected WorkerBase(IStreamStore store, Id streamId, ProgressTracker progressTracker)
        {
            this.store = store.ThrowIfNull(nameof(store));
            this.streamId = streamId.ThrowIfHasNoValue(nameof(streamId));
            this.progressTracker = progressTracker;
        }

        public async Task BeginWorkAsync(int sleepPeriod, CancellationToken token)
        {
            progressTracker.WriteInfo($"InitialSleepPeriod={InitialSleepPeriod}, SleepPeriodBetweenAttempts={sleepPeriod}");
            await Task.Delay(sleepPeriod);

            while (!token.IsCancellationRequested)
            {
                await DoWorkAsync(token);
                await Task.Delay(sleepPeriod);
            }
        }

        protected abstract int InitialSleepPeriod { get; }
        protected abstract Task DoWorkAsync(CancellationToken token);

    }
}
