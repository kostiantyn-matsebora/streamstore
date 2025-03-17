using System.Diagnostics.CodeAnalysis;

namespace StreamStore.ExampleBase.Progress.Model
{
    [ExcludeFromCodeCoverage]
    class ReadCompleted : ProgressInfo
    {
        public ReadCompleted(int actualRevision)
        {
            ActualRevision = actualRevision;
        }

        public int ActualRevision { get; }
    }


}
