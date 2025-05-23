using System.Collections.Concurrent;
using StreamStore.Multitenancy;
using StreamStore.Validation;

namespace StreamStore.InMemory
{
    public sealed class InMemoryStreamStorageProvider : ITenantStreamStorageProvider
    {
        internal ConcurrentDictionary<string, InMemoryStreamStorage> registry = new ConcurrentDictionary<string, InMemoryStreamStorage>();
        readonly IStreamMutationRequestValidator validator;

        public InMemoryStreamStorageProvider(IStreamMutationRequestValidator validator)
        {
           this.validator = validator.ThrowIfNull(nameof(validator));
        }

        
        public IStreamStorage GetStorage(Id tenantId)
        {
            return registry.GetOrAdd(tenantId, _ => new InMemoryStreamStorage(validator));
        }
    }
}
