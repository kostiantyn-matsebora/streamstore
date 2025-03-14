using System.Threading.Tasks;
using System.Threading;

using System.Diagnostics.CodeAnalysis;
using StreamStore.Exceptions;
using System.Diagnostics;
using StreamStore.ExampleBase.Progress;



namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    internal class ReaderToEnd : WorkerBase
    {
        protected override int InitialSleepPeriod => 5_000;
        readonly ReadToEndProgressTracker progressTracker;

        public ReaderToEnd(IStreamStore store, Id streamId, ReadToEndProgressTracker progressTracker) : base(store, streamId, progressTracker)
        {
            this.progressTracker = progressTracker;
        }

        protected override async Task DoWorkAsync(CancellationToken token)
        {
            progressTracker.StartReading();

            try
            {
                var result = await store.ReadToEndAsync(streamId, token);
                progressTracker.ReadEnded(result.MaxRevision);
            }
            catch (StreamNotFoundException)
            {
            }
        }
    }
}
