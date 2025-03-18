using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ShellProgressBar;
using StreamStore.ExampleBase.Progress.Model;
using StreamStore.ExampleBase.Workers;
using StreamStore.Exceptions;


namespace StreamStore.ExampleBase.Progress
{
    [ExcludeFromCodeCoverage]
    public class WorkerRegistry : IObserver<ProgressInfo>
    {
        readonly List<ReaderProgressReporter> readReporters = new List<ReaderProgressReporter>();
        readonly ProgressBar progressBar;

        public WorkerRegistry(StreamStoreConfiguration configuration)
        {
            progressBar = new ProgressBar(1, "Maximum revision of all streams is unknown", new DefaultProgressBarOptions(true));
            progressBar.WriteLine($"Configuration: ReadingMode={configuration.ReadingMode}, ReadingPageSize={configuration.ReadingPageSize}");
        }


        internal ReaderProgressReporter RegisterReader(Reader reader)
        {
            var reporter = new ReaderProgressReporter(this, reader.Id);
            reader.Subscribe(reporter);
            readReporters.Add(reporter);
            return reporter;
        }

        internal WriterProgressReporter RegisterWriter(Writer writer)
        {
            var reporter = new WriterProgressReporter(this, writer.Id);
            writer.Subscribe(reporter);
            writer.Subscribe(this);
            return reporter;
        }

        internal ReaderToEndProgressReporter RegisterReaderToEnd(ReaderToEnd reader)
        {
            var reporter =  new ReaderToEndProgressReporter(this, reader.Id);
            reader.Subscribe(reporter);
            return reporter;
        }

        internal WorkerProgressBar CreateWorkerProgressBar(WorkerIdentifier identifier)
        {
            return new WorkerProgressBar(identifier, progressBar.Spawn(1, identifier.ToString(), new DefaultProgressBarOptions()));
        }
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
            if (error is OptimisticConcurrencyException ex)
            {
                ReportActualRevision((int)ex.ActualRevision!);
            }
        }

        public void OnNext(ProgressInfo value)
        {
            OnProgress((dynamic)value);
        }

        void OnProgress(WriteSucceeded progress)
        {
            ReportActualRevision(progress.ActualRevision);
        }

        void OnProgress(ProgressInfo value)
        {
            // Ignoring any other progresses
        }

        void ReportActualRevision(int actualRevision)
        {
            if (progressBar.MaxTicks < actualRevision)
            {
                SetMaxRevision(actualRevision);
                var @event = new MaximumRevisionRetrieved(actualRevision);
                foreach (var reporter in readReporters)
                {
                    reporter.OnNext(@event);
                }
            }
        }

        void SetMaxRevision(int maxRevision)
        {
            foreach (var reporter in readReporters)
            {
                reporter.SetMaxRevision(maxRevision);
            }
            progressBar.Tick(progressBar.CurrentTick, $"Maximum revision of all streams is {maxRevision}.");
            progressBar.MaxTicks = maxRevision;
        }

    }
}
