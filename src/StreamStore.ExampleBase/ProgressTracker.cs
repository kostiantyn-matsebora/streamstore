

using System;
using System.Collections.Generic;
using System.Diagnostics;
using ShellProgressBar;


namespace StreamStore.ExampleBase
{
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

    public abstract class ProgressTracker
    {
        protected readonly ProgressTrackerFactory factory;
        protected readonly ChildProgressBar progressBar;
        readonly string name;
        long milliseconds = 0;
        public ProgressTracker(ProgressTrackerFactory factory, string name)
        {
            this.factory = factory;
            this.name = name;
            this.progressBar = factory.CreateChildProgressBar(Identifier);
        }


        public void WriteInfo(string message)
        {
            progressBar.WriteLine($"{name}: {message}");
        }

        string Identifier => $"{milliseconds.ToString("D8")} ms {name}";


        protected void WriteProgress(int progress, string message, long? elapsedMilliseconds = null)
        {
            milliseconds = elapsedMilliseconds ?? milliseconds;
            progressBar.Tick(progress, $"{Identifier}: {message}");
        }

        protected void WriteProgress(string message, long? elapsedMilliseconds = null)
        {
            milliseconds = elapsedMilliseconds ?? milliseconds;
            progressBar.Tick($"{Identifier}: {message}");
        }
    }

    public class WriteProgressTracker : ProgressTracker
    {
        readonly Stopwatch stopwatch = new Stopwatch();

        public WriteProgressTracker(ProgressTrackerFactory factory, string name) : base(factory, name)
        {
        }

        public void StartWriting()
        {
            stopwatch.Restart();
        }

        public void WriteSucceeded(int maxRevision, int count)
        {
            stopwatch.Stop();
            factory.ReportWrite(maxRevision);
            WriteProgress($"Completed appending stream to {maxRevision} revision by adding {count} events", stopwatch.ElapsedMilliseconds);

        }

        public void WriteFailed(int maxRevision, string message)
        {
            stopwatch.Stop();
            factory.ReportWriteFail(maxRevision);
            WriteProgress(0, message, stopwatch.ElapsedMilliseconds);
        }


    }

    public class ReadProgressTracker : ProgressTracker
    {
        readonly Stopwatch stopwatch = new Stopwatch();


        public ReadProgressTracker(ProgressTrackerFactory factory, string name) : base(factory, name)
        {
        }

        public void StartReading()
        {
            stopwatch.Restart();
            factory.ReportRead(0);
            WriteProgress(0, $"Start reading...", 0);
        }

        public void ReportRead(int revision)
        {
            factory.ReportRead(revision);
            WriteProgress($"Read events  to revision {revision}", stopwatch.ElapsedMilliseconds);
        }

        public void SetMaxRevision(int revision)
        {
            progressBar.MaxTicks = revision;
        }

        public int Cursor => progressBar.CurrentTick;
    }

    public class ReadToEndProgressTracker : ProgressTracker
    {
        readonly Stopwatch stopwatch = new Stopwatch();

        public ReadToEndProgressTracker(ProgressTrackerFactory factory, string name) : base(factory, name)
        {
        }

        public void ReadEnded(int revision)
        {
            stopwatch.Stop();
            WriteProgress($"Stream read to end, revision is {revision}", stopwatch.ElapsedMilliseconds);
        }

        public void StartReading()
        {
            stopwatch.Restart();
            WriteProgress(0, " Start reading stream to end...", 0);
        }
    }

    class DefaultProgressBarOptions : ProgressBarOptions
    {
        public DefaultProgressBarOptions(bool disableBottomPercentage = true)
        {
            ProgressBarOnBottom = true;
            DisplayTimeInRealTime = true;
            ForegroundColor = ConsoleColor.Yellow;
            BackgroundColor = ConsoleColor.DarkGray;
            DisableBottomPercentage = disableBottomPercentage;
        }
    }
}
