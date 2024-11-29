using System.Diagnostics.CodeAnalysis;

namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    public sealed class InvocationContext
    {
        public InvocationContext(StoreMode mode, string database)
        {
            this.Mode = mode;
            this.Database = database;
        }

        public string Database { get;  }
        public StoreMode Mode { get; }
    }
}
