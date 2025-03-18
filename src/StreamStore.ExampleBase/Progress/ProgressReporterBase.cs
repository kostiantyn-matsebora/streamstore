using System.Diagnostics.CodeAnalysis;
using StreamStore.ExampleBase.Progress.Model;
using StreamStore.ExampleBase.Workers;


namespace StreamStore.ExampleBase.Progress
{
    [ExcludeFromCodeCoverage]
    abstract class ProgressReporterBase
    {
        
        protected readonly WorkerProgressBar progressBar;
        long milliseconds = 0;

        protected ProgressReporterBase(WorkerRegistry factory, WorkerIdentifier identifier)
        {            
            progressBar = factory.CreateWorkerProgressBar(identifier);
        }


        protected void ReportInfoUpdated(InfoUpdated progress)
        {
            progressBar.ShowInfo(progress.Information);
        }

        protected void ReportProgress(int progress, string message, long? elapsed)
        {
            
            milliseconds = elapsed ?? milliseconds;
            progressBar.UpdateProgress(progress, message, milliseconds);
        }

        protected void ReportFailure(int progress, string message, long? elapsed)
        {
            milliseconds = elapsed ?? milliseconds;
            progressBar.ShowError(progress, message, milliseconds);
        }

        protected void ReportFailure(string message, long? elapsed)
        {
            milliseconds = elapsed ?? milliseconds;
            progressBar.ShowError(message, milliseconds);
        }

        protected void ReportCompletion(string message, long? elapsed)
        {
            milliseconds = elapsed ?? milliseconds;
            progressBar.ShowCompleted(message, milliseconds);
        }
    }
}
