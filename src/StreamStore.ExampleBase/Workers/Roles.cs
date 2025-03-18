using System.Diagnostics.CodeAnalysis;


namespace StreamStore.ExampleBase.Workers
{
    [ExcludeFromCodeCoverage]
    static class Roles
    {
        public const string Reader = "reader";
        public const string Writer = "writer";
        public const string ReaderToEnd = "reader-to-end";
    }
}
