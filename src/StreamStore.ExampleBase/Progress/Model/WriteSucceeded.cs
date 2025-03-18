using System.Diagnostics.CodeAnalysis;

namespace StreamStore.ExampleBase.Progress.Model
{
    [ExcludeFromCodeCoverage]
    class WriteSucceeded : ProgressInfo
    {
        public int ActualRevision { get; }
        public int Count { get; }
        public WriteSucceeded(int actualRevision, int count)
        {
            ActualRevision = actualRevision;
            Count = count;
        }
    }
}
