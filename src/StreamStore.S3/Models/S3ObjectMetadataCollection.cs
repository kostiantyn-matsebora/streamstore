
using System.Collections.Generic;
using StreamStore.S3.Client;

namespace StreamStore.S3.Models
{
    internal sealed class S3ObjectMetadataCollection : IS3MetadataCollection
    {
        readonly Dictionary<string, string> metadata = new Dictionary<string, string>();
        public string this[string key] => metadata[key];

        public ICollection<string> Keys => metadata.Keys;

        public IS3MetadataCollection Add(string key, string value)
        {
            metadata.Add(key, value);
            return this;
        }

        public static readonly S3ObjectMetadataCollection Empty = new S3ObjectMetadataCollection();
    }
}
