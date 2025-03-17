using System.Diagnostics.CodeAnalysis;

namespace StreamStore.ExampleBase.Progress.Model
{
    [ExcludeFromCodeCoverage]
    internal class InfoUpdated : ProgressInfo
    {
        public string Information { get; set; }
        public InfoUpdated(string information)
        {
            Information = information;
        }
    }
}
