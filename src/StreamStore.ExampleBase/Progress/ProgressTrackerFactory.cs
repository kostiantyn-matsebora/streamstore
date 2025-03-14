using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ShellProgressBar;


namespace StreamStore.ExampleBase.Progress
{
    [ExcludeFromCodeCoverage]
    public class ProgressTrackerFactory
    {
        List<ReadProgressTracker> readTrackers = new List<ReadProgressTracker>();
        ProgressBar progressBar;

        public ProgressTrackerFactory(StreamStoreConfiguration configuration)
        {
            progressBar = new ProgressBar(1, "Maximum revision of all streams is unknown", new DefaultProgressBarOptions(true));
            progressBar.WriteLine($"Configuration: ReadingMode={configuration.ReadingMode}, ReadingPageSize={configuration.ReadingPageSize}");
        }


        public ReadProgressTracker SpawnReadTracker(string name)
        {
            var tracker = new ReadProgressTracker(this, name);
            readTrackers.Add(tracker);
            return tracker;
        }

        public WriteProgressTracker SpawnWriteTracker(string name)
        {
            return new WriteProgressTracker(this, name);
        }

        public ReadToEndProgressTracker SpawnReadToEndTracker(string name)
        {
            return new ReadToEndProgressTracker(this, name);
        }

        internal ChildProgressBar CreateChildProgressBar(string name)
        {
            return progressBar.Spawn(1, name, new DefaultProgressBarOptions());
        }


        public void ReportRead(int revision)
        {
            if (progressBar.CurrentTick < revision)
            {
                SetMaxReadRevision(revision);
            }
        }

        public void ReportWrite(int maxRevision)
        {
            SetMaxRevision(maxRevision);
        }

        public void ReportWriteFail(int maxRevision)
        {
            SetMaxRevision(maxRevision);
        }

        void SetMaxRevision(int maxRevision)
        {
            foreach (var tracker in readTrackers)
            {
                tracker.SetMaxRevision(maxRevision);
            }
            progressBar.Tick(progressBar.CurrentTick, $"Maximum revision of all streams is {maxRevision}.");
            progressBar.MaxTicks = maxRevision;
        }

        void SetMaxReadRevision(int maxReadRevision)
        {
            progressBar.Tick(maxReadRevision, $"Maximum revision of all streams is {progressBar.MaxTicks}.");
        }

    }
}
