using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using ShellProgressBar;


namespace StreamStore.ExampleBase.Progress
{
    [ExcludeFromCodeCoverage]
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
            progressBar = factory.CreateChildProgressBar(Identifier);
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

    [ExcludeFromCodeCoverage]
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

    [ExcludeFromCodeCoverage]
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

    [ExcludeFromCodeCoverage]
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
}
