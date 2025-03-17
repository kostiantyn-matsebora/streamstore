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
            ReportFailure(1, error.Message, stopwatch.ElapsedMilliseconds);
        }

        public void OnNext(ProgressInfo value)
        {
            OnProgress((dynamic)value);
        }

        void OnProgress(StartWriting progress)
        {
            stopwatch.Restart();
            ReportProgress(0, $"Start writing to stream, expected revision is {progress.ExpectedRevision}...", 0);
        }

        void OnProgress(WriteSucceeded progress)
        {
            stopwatch.Stop();
            ReportWriteCompletion($"Completed appending stream to {progress.ActualRevision} revision by adding {progress.Count} events", stopwatch.ElapsedMilliseconds);
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
