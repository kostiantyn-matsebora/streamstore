using System.Diagnostics.CodeAnalysis;

namespace StreamStore.ExampleBase
{
    [ExcludeFromCodeCoverage]
    public sealed class InvocationContext
    {
        public InvocationContext(StoreMode mode, string storage)
        {
            this.Mode = mode;
            this.Storage = storage;
        }

        public string Storage { get;  }
        public StoreMode Mode { get; }
    }
}
