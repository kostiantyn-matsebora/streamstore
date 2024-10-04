using System;
using System.Collections.Generic;
using Amazon.S3.Model;

namespace StreamStore.S3.AmazonS3
{
    internal class AmazonMetadataCollection : IMetadataCollection
    {
        private readonly MetadataCollection collection;

        public ICollection<string> Keys => collection.Keys;

        public string this[string name] => collection[name];

        public AmazonMetadataCollection(MetadataCollection collection)
        {
            this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }
    }
}
