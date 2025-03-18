using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using StreamStore.ExampleBase.Progress.Model;
using StreamStore.ExampleBase.Workers;


namespace StreamStore.ExampleBase.Progress
{
    [ExcludeFromCodeCoverage]
    class ReaderToEndProgressReporter : ProgressReporterBase, IObserver<ProgressInfo>
    {
        readonly Stopwatch stopwatch = new Stopwatch();

        public ReaderToEndProgressReporter(WorkerRegistry factory, WorkerIdentifier identifier) : base(factory, identifier)
        {
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            ReportFailure(error.Message, stopwatch.ElapsedMilliseconds);
        }


        public void OnNext(ProgressInfo value)
        {
            OnProgress((dynamic)value);
        }

        void OnProgress(StartReading progress)
        {
            stopwatch.Restart();
            ReportProgress(
                progress: 0,
                message: "Start reading...",
                elapsed: 0);
        }

        void OnProgress(ReadCompleted progress)
        {
            ReportCompletion(
                message: $"Stream read to end, revision is {progress.ActualRevision}", 
                elapsed: stopwatch.ElapsedMilliseconds);
        }

        void OnProgress(InfoUpdated progress)
        {
            ReportInfoUpdated(progress);
        }

        void OnProgress(ProgressInfo progress)
        {
            throw new NotImplementedException();
        }
    }
}
