using System.Diagnostics.CodeAnalysis;

namespace StreamStore.ExampleBase.Progress.Model
{
    [ExcludeFromCodeCoverage]
    class ReadSucceeded : ProgressInfo
    {
        public ReadSucceeded(int currentRevision)
        {
            CurrentRevision = currentRevision;
        }

        public int CurrentRevision { get; }
    }


}
