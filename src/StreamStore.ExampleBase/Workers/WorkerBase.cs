using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics.CodeAnalysis;

using System;
using System.Collections.Generic;
using StreamStore.ExampleBase.Progress.Model;
using static System.Collections.Specialized.BitVector32;



namespace StreamStore.ExampleBase.Workers
{
    [ExcludeFromCodeCoverage]
    internal abstract class WorkerBase : IObservable<ProgressInfo>
    {
        protected readonly IStreamStore store;
        protected readonly Id streamId;
        readonly List<IObserver<ProgressInfo>> observers;

        public WorkerIdentifier Id { get; }

        protected WorkerBase(WorkerIdentifier identifier, IStreamStore store, Id streamId)
        {
            this.store = store.ThrowIfNull(nameof(store));
            this.streamId = streamId.ThrowIfHasNoValue(nameof(streamId));
            Id = identifier;
            observers = new List<IObserver<ProgressInfo>>();
        }

        public async Task BeginWorkAsync(int sleepPeriod, CancellationToken token)
        {
            TrackProgress(new InfoUpdated($"InitialSleepPeriod={InitialSleepPeriod}, SleepPeriodBetweenAttempts={sleepPeriod}"));

            await Task.Delay(sleepPeriod);

            while (!token.IsCancellationRequested)
            {
                await DoWorkAsync(token);
                await Task.Delay(sleepPeriod);
            }
        }

        public IDisposable Subscribe(IObserver<ProgressInfo> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        public void TrackProgress(ProgressInfo progress)
        {
            foreach (var observer in observers)
            {
                observer.OnNext(progress);
            }
        }

        public void TrackError(Exception error)
        {
            foreach (var observer in observers)
            {
                observer.OnError(error);
            }
        }

        protected abstract int InitialSleepPeriod { get; }
        protected abstract Task DoWorkAsync(CancellationToken token);

        class Unsubscriber : IDisposable
        {
            readonly List<IObserver<ProgressInfo>> observers;
            readonly IObserver<ProgressInfo> observer;

            public Unsubscriber(List<IObserver<ProgressInfo>> observers, IObserver<ProgressInfo> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            void Dispose(bool disposing)
            {
                if (disposing && observer != null && observers.Contains(observer))
                {
                   observers.Remove(observer);
                }
            }
        }
    }
}
