using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.ExampleBase.Progress;
using StreamStore.Exceptions;


namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    internal class Reader : WorkerBase
    {
        readonly ReadProgressTracker progressTracker;

        public Reader(IStreamStore store, Id streamId, ReadProgressTracker progressTracker) : base(store, streamId, progressTracker)
        {
            this.progressTracker = progressTracker;
        }

        protected override int InitialSleepPeriod => 5_000;

        protected override async Task DoWorkAsync(CancellationToken token)
        {
            IAsyncEnumerator<StreamEvent> enumerator;

            progressTracker.StartReading();
            try
            {
                enumerator = (await store.BeginReadAsync(streamId, token)).GetAsyncEnumerator();
            }
            catch (StreamNotFoundException)
            {
                return;
            }

            bool notEmpty = false;
            int currentRevision = 0;
            do
            {
                notEmpty = await enumerator.MoveNextAsync();
                if (notEmpty)
                {
                    currentRevision = enumerator.Current.Revision;
                    progressTracker.ReportRead(currentRevision);
                }
            } while (notEmpty);

        }
    }
}
