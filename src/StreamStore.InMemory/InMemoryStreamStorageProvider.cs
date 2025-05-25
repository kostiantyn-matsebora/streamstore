using System.Collections.Concurrent;
using StreamStore.Multitenancy;
using StreamStore.Validation;

namespace StreamStore.InMemory
{
    public sealed class InMemoryStreamStorageProvider : ITenantStreamStorageProvider
    {
        internal ConcurrentDictionary<string, InMemoryStreamStorage> registry = new ConcurrentDictionary<string, InMemoryStreamStorage>();
        readonly IDuplicateEventValidator eventValidator;
        readonly IDuplicateRevisionValidator revisionValidator;

        public InMemoryStreamStorageProvider(IDuplicateEventValidator eventValidator, IDuplicateRevisionValidator revisionValidator)
        {
           this.eventValidator = eventValidator.ThrowIfNull(nameof(eventValidator));
           this.revisionValidator = revisionValidator.ThrowIfNull(nameof(revisionValidator));
        }

        
        public IStreamStorage GetStorage(Id tenantId)
        {
            return registry.GetOrAdd(tenantId, _ => new InMemoryStreamStorage(eventValidator, revisionValidator));
        }
    }
}
