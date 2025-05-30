using System.Threading.Tasks;
using System.Threading;

using System.Diagnostics.CodeAnalysis;

using StreamStore.ExampleBase.Progress.Model;
using System.Linq;
using StreamStore.Exceptions.Reading;



namespace StreamStore.ExampleBase.Workers
{
    [ExcludeFromCodeCoverage]
    internal class ReaderToEnd : WorkerBase
    {
        protected override int InitialSleepPeriod => 5_000;

        public ReaderToEnd(ReaderToEndIdentifier identifier, IStreamStore store, Id streamId) : base(identifier, store, streamId)
        {
        }

        protected override async Task DoWorkAsync(CancellationToken token)
        {
            TrackProgress(new StartReading());

            try
            {
                var result = await store.ReadToEndAsync(streamId, token);
                TrackProgress(new ReadCompleted(result.Last().Revision));
            }
            catch (StreamNotFoundException ex)
            {
                TrackError(ex);
            }
        }
    }
}
