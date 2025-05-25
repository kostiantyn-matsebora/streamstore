using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.ExampleBase.Progress.Model;
using StreamStore.Exceptions;


namespace StreamStore.ExampleBase.Workers
{
    [ExcludeFromCodeCoverage]
    internal class Reader : WorkerBase
    {
        public Reader(ReaderIdentifier identifier, IStreamStore store, Id streamId) : base(identifier, store, streamId)
        {
        }

        protected override int InitialSleepPeriod => 5_000;

        protected override async Task DoWorkAsync(CancellationToken token)
        {
            IAsyncEnumerator<IStreamEvent> enumerator;

            TrackProgress(new StartReading());

            try
            {
                enumerator = (await store.BeginReadAsync(streamId, token)).GetAsyncEnumerator();
            }
            catch (StreamNotFoundException ex)
            {
                TrackError(ex);
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
                    TrackProgress(new ReadSucceeded(currentRevision));
                }
            } while (notEmpty);

            TrackProgress(new ReadCompleted(currentRevision));
        }
    }
}
