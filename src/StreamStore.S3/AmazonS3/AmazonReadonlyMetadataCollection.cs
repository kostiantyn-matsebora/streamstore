using System;
using System.Collections.Generic;
using Amazon.S3.Model;
using StreamStore.S3.Client;

namespace StreamStore.S3.AmazonS3
{
    internal class AmazonReadonlyMetadataCollection : IS3ReadonlyMetadataCollection
    {
        private readonly MetadataCollection collection;

        public ICollection<string> Keys => collection.Keys;

        public string this[string name] => collection[name];

        public AmazonReadonlyMetadataCollection(MetadataCollection collection)
        {
            this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }
    }
}
