using System;
using System.Diagnostics.CodeAnalysis;
using ShellProgressBar;
using StreamStore.ExampleBase.Workers;


namespace StreamStore.ExampleBase.Progress
{
    [ExcludeFromCodeCoverage]
    class WorkerProgressBar
    {
        readonly WorkerIdentifier identifier;
        readonly ChildProgressBar progressBar;
        readonly ConsoleColor originalForegroundColor;


        public WorkerProgressBar(WorkerIdentifier identifier, ChildProgressBar progressBar)
        {
            this.identifier = identifier;
            this.progressBar = progressBar;
            originalForegroundColor = progressBar.ForegroundColor;
        }

        public void ShowError(int progress, string message, long milliseconds)
        {
            progressBar.ObservedError = true;
            progressBar.Tick(progress, ComposeMessage(message, milliseconds));
        }
        public void ShowError( string message, long milliseconds)
        {
            progressBar.ObservedError = true;
            progressBar.Tick(progressBar.CurrentTick, ComposeMessage(message, milliseconds));
        }

        public void ShowCompleted(string message, long elapsed)
        {
            progressBar.ForegroundColor = ConsoleColor.Green;
            progressBar.ObservedError = false;
            progressBar.Tick(progressBar.MaxTicks, ComposeMessage(message, elapsed));
        }


        public void ShowInfo(string message)
        {
            progressBar.WriteLine($"{identifier}: {message}");
        }

        public void UpdateProgress(int progress, string message, long elapsed)
        {
            progressBar.ForegroundColor = originalForegroundColor;
            progressBar.ObservedError = false;
            progressBar.Tick(progress, ComposeMessage(message, elapsed));
        }

        string ComposeMessage(string message, long milliseconds)
        {
            return $"{milliseconds.ToString("D8")} ms {identifier}:  {message}";
        }

        public void UpdateMaxValue(int value)
        {
            progressBar.MaxTicks = value;
        }
    }
}
