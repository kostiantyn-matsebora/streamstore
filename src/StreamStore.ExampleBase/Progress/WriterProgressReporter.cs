using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using StreamStore.ExampleBase.Progress.Model;
using StreamStore.ExampleBase.Workers;


namespace StreamStore.ExampleBase.Progress
{
    [ExcludeFromCodeCoverage]
    class WriterProgressReporter : ProgressReporterBase, IObserver<ProgressInfo>
    {
        readonly Stopwatch stopwatch = new Stopwatch();

        public WriterProgressReporter(WorkerRegistry factory, WorkerIdentifier identifier) : base(factory, identifier)
        {
        }

        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            stopwatch.Stop();
            ReportFailure(
                progress: 1, 
                message: error.Message, 
                elapsed: stopwatch.ElapsedMilliseconds);
        }

        public void OnNext(ProgressInfo value)
        {
            OnProgress((dynamic)value);
        }

        void OnProgress(StartWriting progress)
        {
            stopwatch.Restart();
            ReportProgress(
                progress: 0,
                message: $"Start writing to stream, expected revision is {progress.ExpectedRevision}...",
                elapsed: 0);
        }

        void OnProgress(WriteSucceeded progress)
        {
            stopwatch.Stop();
            ReportCompletion(
                message: $"Completed appending stream having {progress.ActualRevision} revision by adding {progress.Count} events", 
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
