using System.Diagnostics.CodeAnalysis;

namespace StreamStore.ExampleBase.Progress.Model
{
    [ExcludeFromCodeCoverage]
    class StartWriting : ProgressInfo
    {
        public int ExpectedRevision { get; }
        public StartWriting(int expectedRevision)
        {
            ExpectedRevision = expectedRevision;
        }
    }
}
