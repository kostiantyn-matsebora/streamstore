
using System.Diagnostics.CodeAnalysis;

namespace StreamStore.ExampleBase.Progress.Model
{
    [ExcludeFromCodeCoverage]
    class MaximumRevisionRetrieved: ProgressInfo
    {
        public int MaximumRevision { get; }

        public MaximumRevisionRetrieved(int maximumRevision)
        {
            MaximumRevision = maximumRevision;
        }
    }
}
