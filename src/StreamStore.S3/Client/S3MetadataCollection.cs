using System.Collections.Generic;

namespace StreamStore.S3.Client
{
    class S3MetadataCollection : IS3ReadonlyMetadataCollection
    {
        readonly Dictionary<string, string> metadata = new Dictionary<string, string>();

        public string this[string name] => metadata[name];

        public ICollection<string> Keys => metadata.Keys;

        public ICollection<KeyValuePair<string, string>> All => metadata;

        public S3MetadataCollection Add(string key, string value)
        {
            Add(key, value);
            return this;
        }
    }
}
